using Microsoft.VisualStudio.TestTools.UnitTesting;

// Make the test parallelization policy explicit (build warning MSTEST0001).
// The suite runs sequentially, matching the previous default behaviour; several
// tests touch shared process state such as console output and are not isolated
// for parallel execution.
[assembly: DoNotParallelize]
