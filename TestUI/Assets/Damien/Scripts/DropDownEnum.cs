using System;
using System.Collections.Generic;
using System.Linq;

public enum DropDownEnum
{
    Choice1,
    Choice2,
    Choice3,
    Choice4,
    Choice5,
    Choice6,
    Choice7,
    Choice8,
    Choice9,
    Choice10,
    Choice11,
    Choice12,
}

public static class DropDownEnumExtensions
{
    public static List<string> GetDropDownEnumList()
    {
        return Enum.GetNames(typeof(DropDownEnum)).ToList();
    }
}