# dotnet-grpc-splitio-sample
Barebones .NET 5 grpc service with Split.io integration which reproduces https://github.com/splitio/dotnet-client/issues/40

## Steps to run and reproduce the issue
To demonstrate the continuous increase in active threadpool threads:
1. Copy appsettings.json to appsettings.Development.json and replace the `SplitIoApiKey` with a valid API key from Split.io.
2. Run the service (either through IDE or CLI).
3. View http://localhost:9001/metrics and http://localhost:9002/metrics in a browser and record the output.
4. Leave the service running for several hours (12+)
5. View http://localhost:9001/metrics and http://localhost:9002/metrics in a browser and record the output.  

Comparing the output of step 5 with step 3, specifically with regards to `process_num_threads`, `dotnet_threadpool_num_threads`, and `coreclr_process_thread_count`,  active threads reported will increase over time even when service is completely idle.  We believe this is due to the background fetching and processing of Split.io client.

## Why is continuously increasing thread count bad?
The CLR threadpool will ideally try to minimize active thread count to only what is necessary (gauged from the work queue) by reusing threads which have completed and are properly disposed.  Having too many active or busy threads will steal from CPU in order to context switch.  When active thread count is above the default configured minThreads, the CLR Threadpool will throttle the rate at which it injects new threads to one thread per 500ms, meaning bursts in traffic will cause significant delays.  See more info on this in the [Microsoft docs](https://docs.microsoft.com/en-us/dotnet/standard/threading/the-managed-thread-pool) and [this helpful gist](https://gist.github.com/JonCole/e65411214030f0d823cb#file-threadpool-md).


## Example results
At the beginning of the test run:
```
coreclr_process_thread_count 53
process_num_threads 54
dotnet_threadpool_num_threads 15
dotnet_threadpool_queue_length_sum 0
dotnet_threadpool_queue_length_count 5
```

After ~12 hours of service idle:
```
coreclr_process_thread_count 124
process_num_threads 124
dotnet_threadpool_num_threads 85
dotnet_threadpool_queue_length_sum 897
dotnet_threadpool_queue_length_count 46737
```

Number of active threads grew linearly during this time frame, when we would expect no thread count growth in the threadpool.
