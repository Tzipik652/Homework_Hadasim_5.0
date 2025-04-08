import React, { FC, JSX, useEffect, useState } from 'react';
import axios from 'axios';
import './AllOrders.scss';
import { BLOrder } from '../../Models/Order';
import { Button, MenuItem, Select, SelectChangeEvent, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, CircularProgress, Alert } from '@mui/material';
import { Warning, HourglassEmpty, Done, CheckCircle } from '@mui/icons-material';

type Status = 'NEW' | 'PROCESSING' | 'COMPLETED';

interface AllOrdersProps {
  supplierId?: number;
  userRole: 'SUPPLIER' | 'MANAGER';
}

const statusColors: { [key: string]: string } = {
  NEW: '#f44336',
  PROCESSING: '#ff9800',
  COMPLETED: '#4caf50',
};

const AllOrders: FC<AllOrdersProps> = ({ supplierId, userRole }) => {
  const [orders, setOrders] = useState<BLOrder[]>([]);
  const [filteredOrders, setFilteredOrders] = useState<BLOrder[]>([]);
  const [statusFilter, setStatusFilter] = useState<string>('ALL');
  const [loading, setLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState<string>("");

  const fetchOrders = async () => {
    setLoading(true);

    try {
      const endpoint = userRole === 'MANAGER'
        ? 'https://localhost:7022/GroceryManagementSystem/Management/all-orders'
        : `https://localhost:7022/GroceryManagementSystem/Suppliers/all-orders/${supplierId}`;

      const response = await axios.get<BLOrder[]>(endpoint);
      setOrders(response.data);
      setFilteredOrders(response.data);
      setLoading(false);

    } catch (error) {
      console.error('Error fetching orders:', error);
    }
  };

  useEffect(() => {
    fetchOrders();
  }, [supplierId, userRole]);

  const handleStatusFilterChange = (event: SelectChangeEvent) => {
    const selected = event.target.value;
    setStatusFilter(selected);
    if (selected === 'ALL') {
      setFilteredOrders(orders);
    } else {
      setFilteredOrders(orders.filter(order => order.statusName === selected));
    }
  };

  const formatDateTime = (dateString: string | Date) => {
    const date = new Date(dateString);
    return date.toLocaleString('he-IL', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
    }).replace(',', '').replace('/', '.');
  };

  const approveOrder = async (orderId: number) => {
    setLoading(true);

    try {
      const endpoint = userRole === 'MANAGER'
        ? `https://localhost:7022/GroceryManagementSystem/Management/complete-order/${orderId}`
        : `https://localhost:7022/GroceryManagementSystem/Suppliers/confirm-order/${orderId}`;

      await axios.put<BLOrder>(endpoint);
      fetchOrders();
      setStatusFilter('ALL');

    } catch (error) {
      console.error('Error approving order:', error);
      setErrorMessage("No order was placed.");

    }finally {
      setLoading(false);

    }
  };

  const statusIcons: { [key in Status]: JSX.Element } = {
    NEW: <Warning style={{ color: 'white' }} />,
    PROCESSING: <HourglassEmpty style={{ color: 'white' }} />,
    COMPLETED: <Done style={{ color: 'white' }} />,
  };

  return (
    <div className="all-orders">
      <div style={{ marginBottom: '1rem' }}>
        <Select value={statusFilter} onChange={handleStatusFilterChange}>
          <MenuItem value="ALL">All</MenuItem>
          <MenuItem value="NEW">New</MenuItem>
          <MenuItem value="PROCESSING">Processing</MenuItem>
          <MenuItem value="COMPLETED">Completed</MenuItem>
        </Select>
      </div>
                {errorMessage && <Alert severity="error">{errorMessage}</Alert>}
      
      {loading ? (
  <div style={{ display: 'flex', justifyContent: 'center', marginTop: '2rem' }}>
    <CircularProgress />
  </div>
) : (
  filteredOrders.length === 0 ? (
<p className="no-orders">No orders for the show.</p>
  ) : (
    <TableContainer component={Paper}>
      <Table aria-label="orders table">
        <TableHead>
          <TableRow>
            <TableCell>Order ID</TableCell>
            <TableCell>Status</TableCell>
            <TableCell>Order Date</TableCell>
            <TableCell>Total Price</TableCell>
            <TableCell>Product</TableCell>
            <TableCell>Quantity</TableCell>
            <TableCell>Actions</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {filteredOrders.map(order => (
            <TableRow key={order.orderId}>
              <TableCell>{order.orderId}</TableCell>
              <TableCell>
                <Button
                  variant="contained"
                  size="small"
                  style={{
                    backgroundColor: statusColors[order.statusName || ''] || '#9e9e9e',
                    marginBottom: '0.5rem',
                  }}
                >
                  {statusIcons[order.statusName as Status]} {order.statusName}
                </Button>
              </TableCell>
              <TableCell>{formatDateTime(order.orderDate)}</TableCell>
              <TableCell>${order.totalPrice}</TableCell>
              <TableCell>{order.productName}</TableCell>
              <TableCell>{order.quantity}</TableCell>
              <TableCell>
                {(order.statusName === 'PROCESSING') && userRole === 'MANAGER' && (
                  <Button
                    variant="outlined"
                    color="primary"
                    onClick={() => approveOrder(order.orderId)}
                    startIcon={<CheckCircle />}
                  >
                    Complete order
                  </Button>
                )}
                {order.statusName === 'NEW' && userRole === 'SUPPLIER' && (
                  <Button
                    variant="outlined"
                    color="primary"
                    onClick={() => approveOrder(order.orderId)}
                    startIcon={<CheckCircle />}
                  >
                    Confirm order
                  </Button>
                )}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  )
)}

    </div>
  );
};

export default AllOrders;
