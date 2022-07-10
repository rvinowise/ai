using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using rvinowise.unity.extensions.pooling;
using rvinowise.rvi.contracts;
using UnityEngine;


public static partial class Unity_extension
{

   

    public static int get_n_frames(
        this UnityEngine.U2D.Animation.SpriteLibrary in_library
    ) {
        UnityEngine.U2D.Animation.SpriteLibraryAsset asset = in_library.spriteLibraryAsset;
        String category = asset.GetCategoryNames().First();
        return asset.GetCategoryLabelNames(category).Count();

    }
    
}
