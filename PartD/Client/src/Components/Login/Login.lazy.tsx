import React, { JSX, lazy, Suspense } from 'react';

const LazyLogin = lazy(() => import('./Login'));

const Login = (props: JSX.IntrinsicAttributes & { children?: React.ReactNode; }) => (
  <Suspense fallback={null}>
    <LazyLogin onLoginSuccess={(role: string, id: number) => {
        
      }}{...props} />
  </Suspense>
);

export default Login;
