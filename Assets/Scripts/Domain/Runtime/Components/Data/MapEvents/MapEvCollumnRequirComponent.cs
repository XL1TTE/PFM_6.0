using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct MapEvCollumnRequirComponent : IComponent
{
    /// <summary>
    /// This component works as such: 
    /// it reads a bool flag and if it is true (count starts at zero) -> it adds an int variable to zero, which will now be the max point.
    /// That means that all points before that (including the limit) can be used to be occupied by this event
    /// If the flag is set to opposite then the offset will be negated from the MAX amount of collumns (ie - counting from the end) which will be the available range.
    /// </summary>
    public bool count_start_from_zero;
    /// <summary>
    /// Essentially - if flag = true, then count from start and that is available collumns, if flag = false, then count from end
    /// </summary>
    public byte count_offset;
    /// <summary>
    /// This is an alternative way of setting the offset, if this variable is set then max collumn count is increased by this variable is added to base offset
    /// In other words - this is a way to add offset value based on the available amount of collumns
    /// </summary>
    public float count_offset_percentile;

    // Example use:
    // 
    // With<MapEvCollumnRequirComponent>(new MapEvCollumnRequirComponent
    // {
    //     count_start_from_zero = true,
    //     count_offset = 2,
    //     count_offset_percentile = 0.4f
    // });
    // 
    // the count will start from zero, will use 40% of the max collumn count and add 2 on top of that
    // so if there are 10 collumns the first 10 * 0.4 + 2 = 6 can be taken by this event
}