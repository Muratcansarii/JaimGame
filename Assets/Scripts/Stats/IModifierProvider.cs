// This file has ImodifierProvider and we use it many place in Stats part of our game

using System.Collections.Generic;

namespace JAIM.Stats // this namespace holds attributes about Stats
{
    public interface IModifierProvider // provides modifier
    {
        IEnumerable<float> GetAdditiveModifiers(Stat stat); // arranges additive modifier
        IEnumerable<float> GetPercentageModifiers(Stat stat); // arranges percentage of modifier
    }
}