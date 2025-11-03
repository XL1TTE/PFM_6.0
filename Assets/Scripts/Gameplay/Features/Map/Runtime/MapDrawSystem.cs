using Domain.Map.Components;
using Domain.Map.Providers;
using Domain.Map.Requests;
using Scellecs.Morpeh;
using TMPro;
using UnityEngine;

namespace Gameplay.Map.Systems
{
    public sealed class MapDrawSystem : MonoBehaviour, ISystem
    {
        public World World { get; set; }

        public Request<MapDrawVisualsRequest> req_draw;

        private Filter filterPos;
        private Filter filterId;

        private Filter filterBG;

        private Stash<TagMapNodeBinder> nodeBinderStash;

        private Stash<MapNodePositionComponent> nodePosStash;
        private Stash<MapNodeIdComponent> nodeIdStash;
        private Stash<MapNodeNeighboursComponent> nodeNeighbStash;

        public Stash<MapNodeEventType> nodeEventTypeStash;
        public Stash<MapNodeEventId> nodeEventIdStash;

        private Stash<MapBGComponent> bgStash;

        private Transform LinesContainer;
        private Transform NodesContainer;
        private Transform BGContainer;

        private GameObject bgPrefab;
        private GameObject nodePrefab;
        private Material lineMaterial;
        private LineRenderer lineRenderer;


        private Sprite icon_lab;
        private Sprite icon_battle;
        private Sprite icon_text;
        private Sprite icon_boss;

        public void OnAwake()
        {

            Debug.Log("NodeDrawSys is Awake");
            req_draw = World.GetRequest<MapDrawVisualsRequest>();

            this.nodeBinderStash = this.World.GetStash<TagMapNodeBinder>();

            this.filterPos = this.World.Filter.With<MapNodePositionComponent>().Build();
            this.filterId = this.World.Filter.With<MapNodeIdComponent>().Build();


            this.nodePosStash = this.World.GetStash<MapNodePositionComponent>();
            this.nodeIdStash = this.World.GetStash<MapNodeIdComponent>();
            this.nodeNeighbStash = this.World.GetStash<MapNodeNeighboursComponent>();

            this.nodeEventTypeStash = this.World.GetStash<MapNodeEventType>();
            this.nodeEventIdStash = this.World.GetStash<MapNodeEventId>();

            this.filterBG = this.World.Filter.With<MapBGComponent>().Build();
            this.bgStash = this.World.GetStash<MapBGComponent>();

            LinesContainer = new GameObject("LinesContainer").transform;
            NodesContainer = new GameObject("NodesContainer").transform;
            BGContainer = new GameObject("BGContainer").transform;


            icon_lab = Resources.Load<Sprite>("Map/EventIcons/UI_Map_Lab");
            icon_battle = Resources.Load<Sprite>("Map/EventIcons/UI_Map_Battle");
            icon_text = Resources.Load<Sprite>("Map/EventIcons/UI_Map_Random");
            icon_boss = Resources.Load<Sprite>("Map/EventIcons/UI_Map_Boss");


            nodePrefab = Resources.Load<GameObject>("Map/Prefabs/MapNodePrefab");
            bgPrefab = Resources.Load<GameObject>("Map/Prefabs/MapBGGeneralPrefab");
            lineMaterial = Resources.Load<Material>("Art/Materials/DottedLine_Material");

        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_draw.Consume())
            {

                foreach (var entity in this.filterPos)
                {
                    ref var nodePosComponent = ref nodePosStash.Get(entity);
                    ref var nodeIdComponent = ref nodeIdStash.Get(entity);
                    ref var nodeNeighbComponent = ref nodeNeighbStash.Get(entity);

                    ref var nodeEventType = ref nodeEventTypeStash.Get(entity);
                    ref var nodeEventId = ref nodeEventIdStash.Get(entity);



                    var prefabedNode = Instantiate(nodePrefab,
                        new Vector3(
                            nodePosComponent.node_x + nodePosComponent.node_x_offset,
                            nodePosComponent.node_y + nodePosComponent.node_y_offset,
                            0),
                        Quaternion.identity);

                    prefabedNode.GetComponentInChildren<TextMeshPro>().text = nodeIdComponent.node_id.ToString() + "\n" + nodeEventId.event_id.ToString();

                    var pref_ent = prefabedNode.GetComponent<MapNodeBinderProvider>().Entity;

                    if (nodeBinderStash.Has(pref_ent))
                    {
                        ref var nodeBinderComponent = ref nodeBinderStash.Get(pref_ent);
                        nodeBinderComponent.node_id = nodeIdComponent.node_id;
                    }

                    var chosen_spr = icon_lab;

                    switch (nodeEventType.event_type)
                    {
                        case EVENT_TYPE.LAB:
                            chosen_spr = icon_lab;
                            break;
                        case EVENT_TYPE.TEXT:
                            chosen_spr = icon_text;
                            break;
                        case EVENT_TYPE.BATTLE:
                            chosen_spr = icon_battle;
                            break;
                        case EVENT_TYPE.BOSS:
                            chosen_spr = icon_boss;
                            break;
                    }

                    prefabedNode.GetComponentInChildren<SpriteRenderer>().sprite = chosen_spr;


                    foreach (var neighbour in this.filterId)
                    {
                        ref var nodeNeighbPosComponent = ref nodePosStash.Get(neighbour);
                        ref var nodeNeighbIdComponent = ref nodeIdStash.Get(neighbour);


                        if (nodeNeighbComponent.node_neighbours.Contains(nodeNeighbIdComponent.node_id)
                        && (nodeNeighbPosComponent.node_collumn < nodePosComponent.node_collumn))
                        {
                            //For creating line renderer object
                            lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();

                            // current iteration - All thanks to Max! So go and thank him for this amazing shader graph work
                            lineRenderer.sortingLayerName = "MapLineLayer";
                            lineRenderer.sortingOrder = 0;
                            
                            lineRenderer.material = lineMaterial;
                            
                            //lineRenderer.widthMultiplier = 3;
                            
                            lineRenderer.startWidth = 20.0f;
                            lineRenderer.endWidth = 20.0f;
                            
                            lineRenderer.textureScale = new Vector2 { x = 0.3f, y = 6 };
                            lineRenderer.textureMode = LineTextureMode.Tile;
                            
                            lineRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                            
                            //For drawing line in the world space, provide the x,y,z values
                            lineRenderer.SetPosition(0, new Vector3(
                                nodePosComponent.node_x + nodePosComponent.node_x_offset,
                                nodePosComponent.node_y + nodePosComponent.node_y_offset, 0)
                                ); //x,y and z position of the starting point of the line
                            lineRenderer.SetPosition(1, new Vector3(
                                nodeNeighbPosComponent.node_x + nodeNeighbPosComponent.node_x_offset,
                                nodeNeighbPosComponent.node_y + nodeNeighbPosComponent.node_y_offset, 0)
                                ); //x,y and z position of the end point of the line
                            
                            lineRenderer.transform.SetParent(LinesContainer, true);
                        }

                    }
                    

                    prefabedNode.transform.SetParent(NodesContainer, true);
                }

                foreach (var bg in filterBG)
                {
                    ref var bgComponent = ref bgStash.Get(bg);
                    var scaleChange = new Vector3((bgComponent.scale_x), 1f, 1f);
                    var bg_instance = Instantiate(bgPrefab, new Vector3(bgComponent.pos_x, bgComponent.pos_y, 0), Quaternion.identity);
                    bg_instance.transform.localScale = scaleChange;
                    bg_instance.GetComponent<SpriteRenderer>().sprite = bgComponent.sprite;
                    bg_instance.GetComponent<SpriteRenderer>().sortingOrder = bgComponent.layer;

                    bg_instance.transform.SetParent(BGContainer, true);
                }
            
            }

        }

        public void Dispose()
        {
            //Debug.Log("NodeDrawSys is Disposing");
            //throw new System.NotImplementedException();
        }

    }

}