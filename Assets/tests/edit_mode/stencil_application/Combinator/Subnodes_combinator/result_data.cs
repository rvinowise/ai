using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using rvinowise.ai.simple.mapping_stencils;
using UnityEngine;
using UnityEngine.TestTools;

namespace rvinowise.ai.unit_tests.generator_of_order_sequences {


public partial class initialised_correctly_for_generation {


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