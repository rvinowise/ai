#define RVI_CONTRACTS

using System;
using System.Diagnostics;

namespace rvinowise.contracts {
public class Contract {
    
    public static void Requires( bool condition, string message="")
    {
#if RVI_CONTRACTS
        #region debug
        if (!condition) {
            Debug.Assert(condition, message);
        }
        #endregion
#endif
    }

    public static void Requires<TException>( bool condition, string message="") 
    where TException: Exception, new()
    {
#if RVI_CONTRACTS
            if (!condition) {
                throw new TException();
            }
#endif
    }
    
    /* not strict requirement, but most logical use of the code.
     if broken, something is not optimal */
    public static void Assume( bool condition, string message="")
    {
#if RVI_CONTRACTS
        #region debug
        if (!condition) {
            Debug.Assert(condition, message);
        }
        #endregion
#endif
    }
    
    
    /* a substitute for native asserts, because they don't stop at breakpoints */
    public static void Assert( bool condition, string message="")
    {
#if RVI_CONTRACTS
        #region debug
        if (!condition) {
            Debug.Assert(condition, message);
        }
        #endregion
#endif
    }
    
    /* the postcondition before exit a function */
    public static void Ensures( bool condition, string message="")
    {
#if RVI_CONTRACTS
        #region debug
        if (!condition) {
            Debug.Assert(condition, message);
        }
        #endregion
#endif
    }
}
}