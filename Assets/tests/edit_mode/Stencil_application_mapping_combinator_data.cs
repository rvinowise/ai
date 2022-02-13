using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using rvinowise.ai.unity.mapping_stencils;
using UnityEngine;
using UnityEngine.TestTools;

namespace tests {


public partial class Stencil_application_mapping_combinator {


    private int[][] result_combinations = {
        new[] {2,1,0},
        new[] {3,1,0},
        new[] {4,1,0},
        
        new[] {1,2,0},
        new[] {3,2,0},
        new[] {4,2,0},
        
        new[] {1,3,0},
        new[] {2,3,0},
        new[] {4,3,0},
        
        new[] {1,4,0},
        new[] {2,4,0},
        new[] {3,4,0},
        
        new[] {2,0,1},
        new[] {3,0,1},
        new[] {4,0,1},
        
        new[] {0,2,1},
        new[] {3,2,1},
        new[] {4,2,1},
        
        new[] {0,3,1},
        new[] {2,3,1},
        new[] {4,3,1},
        
        new[] {0,4,1},
        new[] {1,4,1},
        new[] {3,4,1},
        
        new[] {1,0,2},
        new[] {3,0,2},
        new[] {4,0,2},
        
        new[] {0,1,2},
        new[] {3,1,2},
        new[] {4,1,2},
        
        new[] {0,3,2},
        new[] {1,3,2},
        new[] {4,3,2},
        
        new[] {0,4,2},
        new[] {1,4,2},
        new[] {3,4,2},
        
        new[] {1,0,3},
        new[] {2,0,3},
        new[] {4,0,3},
        
        new[] {0,1,3},
        new[] {2,1,3},
        new[] {4,1,3},
        
        new[] {0,2,3},
        new[] {1,2,3},
        new[] {4,2,3},
        
        new[] {0,4,3},
        new[] {1,4,3},
        new[] {2,4,3},
        
        new[] {1,0,4},
        new[] {2,0,4},
        new[] {3,0,4},
        
        new[] {0,1,4},
        new[] {2,1,4},
        new[] {3,1,4},
        
        new[] {0,2,4},
        new[] {1,2,4},
        new[] {3,2,4},
        
        new[] {0,3,4},
        new[] {1,3,4},
        new[] {2,3,4},
        
    };
    

    /* outer array - combinations;
       array inside - figure indexes; 
       innermost array - taken indexes of occurences of that figure
     */
    private int[][][] result_total_combinations = {
        
        //iteration 0 of the second figure
        new[] {
            new[] {1,0},
            new[] {2,1,0}
        },
        new[] {
            new[] {2,0},
            new[] {2,1,0}
        },
        new[] {
            new[] {3,0},
            new[] {2,1,0}
        },
        
        new[] {
            new[] {0,1},
            new[] {2,1,0}
        },
        new[] {
            new[] {2,1},
            new[] {2,1,0}
        },
        new[] {
            new[] {3,1},
            new[] {2,1,0}
        },
        
        new[] {
            new[] {0,2},
            new[] {2,1,0}
        },
        new[] {
            new[] {1,2},
            new[] {2,1,0}
        },
        new[] {
            new[] {3,2},
            new[] {2,1,0}
        },
        
        new[] {
            new[] {0,3},
            new[] {2,1,0}
        },
        new[] {
            new[] {1,3},
            new[] {2,1,0}
        },
        new[] {
            new[] {2,3},
            new[] {2,1,0}
        },
        
        //iteration 1 of the second figure
        new[] {
            new[] {1,0},
            new[] {1,2,0}
        },
        new[] {
            new[] {2,0},
            new[] {1,2,0}
        },
        new[] {
            new[] {3,0},
            new[] {1,2,0}
        },
        
        new[] {
            new[] {0,1},
            new[] {1,2,0}
        },
        new[] {
            new[] {2,1},
            new[] {1,2,0}
        },
        new[] {
            new[] {3,1},
            new[] {1,2,0}
        },
        
        new[] {
            new[] {0,2},
            new[] {1,2,0}
        },
        new[] {
            new[] {1,2},
            new[] {1,2,0}
        },
        new[] {
            new[] {3,2},
            new[] {1,2,0}
        },
        
        new[] {
            new[] {0,3},
            new[] {1,2,0}
        },
        new[] {
            new[] {1,3},
            new[] {1,2,0}
        },
        new[] {
            new[] {2,3},
            new[] {1,2,0}
        },
        
        //iteration 2 of the second figure
        new[] {
            new[] {1,0},
            new[] {2,0,1}
        },
        new[] {
            new[] {2,0},
            new[] {2,0,1}
        },
        new[] {
            new[] {3,0},
            new[] {2,0,1}
        },
        
        new[] {
            new[] {0,1},
            new[] {2,0,1}
        },
        new[] {
            new[] {2,1},
            new[] {2,0,1}
        },
        new[] {
            new[] {3,1},
            new[] {2,0,1}
        },
        
        new[] {
            new[] {0,2},
            new[] {2,0,1}
        },
        new[] {
            new[] {1,2},
            new[] {2,0,1}
        },
        new[] {
            new[] {3,2},
            new[] {2,0,1}
        },
        
        new[] {
            new[] {0,3},
            new[] {2,0,1}
        },
        new[] {
            new[] {1,3},
            new[] {2,0,1}
        },
        new[] {
            new[] {2,3},
            new[] {2,0,1}
        },
        
        //iteration 3 of the second figure
        new[] {
            new[] {1,0},
            new[] {0,2,1}
        },
        new[] {
            new[] {2,0},
            new[] {0,2,1}
        },
        new[] {
            new[] {3,0},
            new[] {0,2,1}
        },
        
        new[] {
            new[] {0,1},
            new[] {0,2,1}
        },
        new[] {
            new[] {2,1},
            new[] {0,2,1}
        },
        new[] {
            new[] {3,1},
            new[] {0,2,1}
        },
        
        new[] {
            new[] {0,2},
            new[] {0,2,1}
        },
        new[] {
            new[] {1,2},
            new[] {0,2,1}
        },
        new[] {
            new[] {3,2},
            new[] {0,2,1}
        },
        
        new[] {
            new[] {0,3},
            new[] {0,2,1}
        },
        new[] {
            new[] {1,3},
            new[] {0,2,1}
        },
        new[] {
            new[] {2,3},
            new[] {0,2,1}
        },
        
        //iteration 4 of the second figure
        new[] {
            new[] {1,0},
            new[] {1,0,2}
        },
        new[] {
            new[] {2,0},
            new[] {1,0,2}
        },
        new[] {
            new[] {3,0},
            new[] {1,0,2}
        },
        
        new[] {
            new[] {0,1},
            new[] {1,0,2}
        },
        new[] {
            new[] {2,1},
            new[] {1,0,2}
        },
        new[] {
            new[] {3,1},
            new[] {1,0,2}
        },
        
        new[] {
            new[] {0,2},
            new[] {1,0,2}
        },
        new[] {
            new[] {1,2},
            new[] {1,0,2}
        },
        new[] {
            new[] {3,2},
            new[] {1,0,2}
        },
        
        new[] {
            new[] {0,3},
            new[] {1,0,2}
        },
        new[] {
            new[] {1,3},
            new[] {1,0,2}
        },
        new[] {
            new[] {2,3},
            new[] {1,0,2}
        },
        
        //iteration 5 of the second figure
        new[] {
            new[] {1,0},
            new[] {0,1,2}
        },
        new[] {
            new[] {2,0},
            new[] {0,1,2}
        },
        new[] {
            new[] {3,0},
            new[] {0,1,2}
        },
        
        new[] {
            new[] {0,1},
            new[] {0,1,2}
        },
        new[] {
            new[] {2,1},
            new[] {0,1,2}
        },
        new[] {
            new[] {3,1},
            new[] {0,1,2}
        },
        
        new[] {
            new[] {0,2},
            new[] {0,1,2}
        },
        new[] {
            new[] {1,2},
            new[] {0,1,2}
        },
        new[] {
            new[] {3,2},
            new[] {0,1,2}
        },
        
        new[] {
            new[] {0,3},
            new[] {0,1,2}
        },
        new[] {
            new[] {1,3},
            new[] {0,1,2}
        },
        new[] {
            new[] {2,3},
            new[] {0,1,2}
        },
    };
}


}