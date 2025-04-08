import React, { JSX, lazy, Suspense } from 'react';

const LazyProductsManager = lazy(() => import('./ProductsManager'));

const ProductsManager = (props: JSX.IntrinsicAttributes & { children?: React.ReactNode; }) => (
  <Suspense fallback={null}>
    <LazyProductsManager  supplierId={0} role={'SUPPLIER'} {...props} />
  </Suspense>
);

export default ProductsManager;
