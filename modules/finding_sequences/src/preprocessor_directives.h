//#ifdef FINDING_SEQUENCES_EXPORTS
#ifdef finding_sequences_EXPORTS
    #define DLLEXPORT __declspec(dllexport)
#else
    #define DLLEXPORT __declspec(dllimport)
#endif