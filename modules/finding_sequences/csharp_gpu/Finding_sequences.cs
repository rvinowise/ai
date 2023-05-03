namespace rvinowise.ai;

using ILGPU;
using ILGPU.Runtime;
using System;

using Xunit;

public class Finding_sequences {
    static void Kernel(Index1D i, ArrayView<int> data, ArrayView<int> output) {
        output[i] = data[i % data.Length];
    }

    static List<Interval> repeated_pair(
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
        loadedKernel((int)deviceOutput.Length, deviceData.View, deviceOutput.View);

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
    
    [Fact]
    public void try_repeated_pair() {
        var a_intervals = new Interval[] {
            new Interval(0, 1),
            new Interval(10, 11)
        };
        var b_intervals = new Interval[] {
            new Interval(5, 6),
            new Interval(15, 16)
        };
        var result = repeated_pair(a_intervals, b_intervals);
    }
}
