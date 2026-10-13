namespace Project.Generation
{
    /// <summary>
    /// 概念图节点和房间模板共用的用途。数值要保持稳定，资产里存的是整数。
    /// </summary>
    public enum RoomKind
    {
        Start = 0,
        Combat = 1,
        Elite = 2,
        Shop = 3,
        Boss = 4,
        Passage = 5
    }

    public enum DoorDirection
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3
    }
}
