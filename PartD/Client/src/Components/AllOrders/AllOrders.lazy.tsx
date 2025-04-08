import React, { JSX, lazy, Suspense } from 'react';

const LazyAllOrders = lazy(() => import('./AllOrders'));

const AllOrders = (props: JSX.IntrinsicAttributes & { children?: React.ReactNode; }) => (
  <Suspense fallback={null}>
    <LazyAllOrders userRole={'SUPPLIER'} supplierId={0} {...props} />
  </Suspense>
);

export default AllOrders;
