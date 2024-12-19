## Default Built-in Pipeline (BRP)
*Honestly just use URP or HDRP, but anyways*
- #### The basics of GPU hardware
GPU's execute shader functions by partitioning its work into groups, scheduling them to
run independently and in parallel.
**GPU Threads**
Every CUDA thread is associated with an index to access memory locations.
**Groups**
A group consists of a number of threads that perform the same calculations but with
different input. Threads in a same group have shared memory. 
**Threading and Dispatching**
We can define the dimensions of a work group via numthreads(a,b,c)
```cs
// On Kernel function execution
[numthreads(a, b, c)]
```
This creates a work group of of a threads in the x dimension (rows), b threads in the y
dimension (columns), c threads of depth (overlapping matrices)
![[Compute Shaders (BRP, URP, HDRP) 1.png]]
Alternatively, a better convention for doing this would be as follows:
```cs
var threads = 16;
var thread_depth = 1;
// On Kernel function execution
[numthreads(thread, thread, thread_depth)]
```
We can then create a grid of work groups by calling Dispatch()
```cs
// On backend function execution
computeShader.Dispatch(kernelIndex, d, e, f);
```
Thereby we end up with the following grid
![[Groups.png]]
Again, the code would look something along these lines:
```cs
var groups = 8;
var group_depth = 1;
computeShader.Dispatch(groups, groups, group_depth);
```
The total number of cells can calculated by:
$(threads \cdot threads \cdot threadDepth) \cdot (groups \cdot groups \cdot groupDepth)$
**GPU Warps / Thread Blocks**
GPU Processors are organized in Warps (Nvidia), Groups are the "software version" of Warps as the hardware drivers take care of mapping them to Wraps such that each Group is allocated in one Warp. It is useful then that each group occupies as much of each Warp as possible (Occupancy) to increase bandwidth, reduce memory latency and share memory between threads.

By doing the following dispatch:
```cs
numthreads(1,1,1)
Dispatch(kernelIndex, 4, 2, 1)
```
We end up with wasting most of the GPU's parallelization and thus its whole purpose:
![[Pasted image 20240729111206.png|325]]
Each *group* is being mapped to a thread block, which might okay since there's only 4 blocks being active at one same time and only 12 total SM processors, but to avoid memory latency not only we want to spread the work groups across all the SM processors, we want to cover all of them or even over-assign so that the drivers can efficiently assign multiple streams to maximize each processor's occupancy and dramatically improving performance.
Not only that, each Warp is only assigning a *single thread* to the processor wasting the GPU's SIMT capabilities, the number of threads that the SM processor can run simultaneously: the Thread Warp Size. In NVIDIA's case, that number is usually 32 threads, in AMD each Wavefront can run 64 threads. So generally working with threads in *multiples of 32* is a good idea.

**Optimizing Dispatches**
Given the following situation:
```cs
// Pixel res (16x16)
resolution = 16;
threads = 8;
// Assign 64 threads to each group
numthreads(threads, threads, 1)
```
We could maximize our Dispatch in this way:
```cs
// Pixel res (16x16)
resolution = 16;
threads = 8;
// Assign 64 threads to each group
numthreads(threads, threads, 1)
// Dispatch 16/8 * 16/8 = 4 groups of 64 threads each
Dispatch(resoltion/threads, resolution/threads, 1)
```
This way, we are dispatching 4 work groups of 64 threads each for a total of 256.
Since each block is 32 threads the hardware will probably end up dividing our 64 work group into two warps of 32 threads instead and thus ending up with 8 warps total which is *probably okay*, since we want to use as many SM processors as possible and we are assigning each pixel to a thread and filling as many threads and groups as we can with this resolution..
## Ultra Resolution Pipeline (URP)
