namespace rvinowise.ai;

using ILGPU;
using ILGPU.Runtime;
using System;

using Xunit;

public static class Finding_many_repetitions_csharp_gpu {
    static void Kernel(Index1D i, ArrayView<int> data, ArrayView<int> output) {
        output[i] = data[i % data.Length];
    }

    public static List<Interval> repeated_pair(
        Interval[] a_appearances,
        Interval[] b_appearances
    ) {
        var context = Context.CreateDefault();
        var accelerator = context.GetPreferredDevice(preferCPU: false)
                                  .CreateAccelerator(context);

        var deviceData = accelerator.Allocate1D(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        var deviceOutput = accelerator.Allocate1D<int>(10_000);

        // load / precompile the kernel
        var loadedKernel = 
            accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<int>, ArrayView<int>>(Kernel);

        // finish compiling and tell the accelerator to start computing the kernel
        loadedKernel((Index1D)deviceOutput.Length, deviceData.View, deviceOutput.View);

        // wait for the accelerator to be finished with whatever it's doing
        // in this case it just waits for the kernel to finish.
        accelerator.Synchronize();

        // moved output data from the GPU to the CPU for output to console
        int[] hostOutput = deviceOutput.GetAsArray1D();

        for(int i = 0; i < 50; i++)
        {
            Console.Write(hostOutput[i]);
            Console.Write(" ");
        }

        accelerator.Dispose();
        context.Dispose();

        return new List<Interval>();
    }
    
    public static List<Sequence_appearances> many_repetitions(
        IEnumerable<Sequence_appearances> sequence_appearances
    ) {
        return new List<Sequence_appearances>();
    }

    public static List<Sequence_appearances> all_repetitions(
        IEnumerable<Sequence_appearances> sequence_appearances
    ) {
        return new List<Sequence_appearances>();
    }
}



public class Finding_many_repetitions_csharp_gpu_test {
    [Fact]
    public void many_repetitions() {
        
        var initial_appearances =
            built.Event_batches.to_sequence_appearances(
            built.Event_batches.from_text(
            "a1b2c3a45bc"
    //       a b c
    //             a  bc
    //mom:   0123456789¹1    
            ));

        var result = Finding_many_repetitions_csharp_gpu.many_repetitions(initial_appearances);
    }
}