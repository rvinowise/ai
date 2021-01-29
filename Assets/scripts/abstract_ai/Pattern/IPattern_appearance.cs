
using System.Numerics;

namespace rvinowise.ai.patterns {
public interface IPattern_appearance {

    IPattern pattern{get;}

    IAppearance_start start{get;}
    IAppearance_end end{get;}
   
    BigInteger start_moment{get;}
    BigInteger end_moment{get;}

    bool is_entirely_before(IPattern_appearance appearance);
}
}