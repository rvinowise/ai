// namespace rvinowise.ai

// module ``Finding_repetitions(gpu)``=
//     open ILGPU
//     open ILGPU.Runtime
//     open System

    
//     let kernel(
//         i:Index1D,
//         data:ArrayView<int>,
//         output: ArrayView<int>
//     )=
//         output[i] <- data[(int64 i.X) % data.Length]
//         ()
    
//     let repeated_pair 
//         (a_appearances: Interval array)
//         (b_appearances: Interval array)
//         =
        
//         let context = Context.CreateDefault()
//         let accelerator = context.GetPreferredDevice(false)
//                                   .CreateAccelerator(context)
        
//         let deviceData = accelerator.Allocate1D([| 0; 1; 2; 3; 4; 5; 6; 7; 8; 9 |])
//         let deviceOutput = accelerator.Allocate1D<int>(10_000);
        
//         //let loadedKernel = 
//         accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<int>, ArrayView<int>>(kernel)
//         |>ignore
        
        
//         let found_pairs = ResizeArray()
//         found_pairs
        