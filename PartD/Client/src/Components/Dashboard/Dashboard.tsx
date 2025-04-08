import React, { useState } from 'react';
import { Box, Button, Typography, Stack } from '@mui/material';
import ProductsManager from '../ProductsManager/ProductsManager';
import AllOrders from '../AllOrders/AllOrders';

interface DashboardProps {
  role: 'SUPPLIER' | 'MANAGER';
  supplierId: number;
}

const Dashboard: React.FC<DashboardProps> = ({ role, supplierId }) => {
  const [currentTab, setCurrentTab] = useState<'products' | 'orders'>('products');

  return (
    <Box sx={{ padding: 2 }}>

      <Stack direction="row" spacing={2} mb={3}>
        <Button
          variant={currentTab === 'products' ? 'contained' : 'outlined'}
          onClick={() => setCurrentTab('products')}
        >
          Products
        </Button>
        <Button
          variant={currentTab === 'orders' ? 'contained' : 'outlined'}
          onClick={() => setCurrentTab('orders')}
        >
          Orders
        </Button>
      </Stack>

      {currentTab === 'products' && (
        <ProductsManager supplierId={supplierId} role={role} />
      )}

      {currentTab === 'orders' && (
        <AllOrders userRole={role} supplierId={role === 'SUPPLIER' ? supplierId : undefined} />
      )}
    </Box>
  );
};

export default Dashboard;
