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


