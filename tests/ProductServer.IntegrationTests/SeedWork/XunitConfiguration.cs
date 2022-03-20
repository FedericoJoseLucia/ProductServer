using Xunit;

// Run tests sequentially to prevent database locks
[assembly: CollectionBehavior(DisableTestParallelization = true)]