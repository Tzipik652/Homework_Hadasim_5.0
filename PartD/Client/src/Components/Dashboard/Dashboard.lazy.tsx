import React, { Suspense, lazy } from 'react';

// Lazy load the component
const LazyDashboard = lazy(() => import('./Dashboard'));

// Your component that uses the lazy-loaded component
const SomeComponent = () => {
  const supplierId = 123; // Example supplierId

  return (
    <Suspense fallback={null}>
      <LazyDashboard />
    </Suspense>
  );
};
