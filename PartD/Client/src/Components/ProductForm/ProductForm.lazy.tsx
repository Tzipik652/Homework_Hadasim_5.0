import React, { JSX, lazy, Suspense } from 'react';

const LazyProductForm = lazy(() => import('./ProductForm'));

const ProductForm = (props: JSX.IntrinsicAttributes & { children?: React.ReactNode; }) => (
  <Suspense fallback={null}>
    <LazyProductForm open={false} onClose={function (): void {
      throw new Error('Function not implemented.');
    } } onSuccess={function (): void {
      throw new Error('Function not implemented.');
    } } supplierId={0} {...props} />
  </Suspense>
);

export default ProductForm;
