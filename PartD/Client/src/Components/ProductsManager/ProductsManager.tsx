import React, { FC, useEffect, useState } from 'react';
import './ProductsManager.scss';
import axios from 'axios';
import {
  Box,
  Button,
  Typography,
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Stack,
  TextField,
  CircularProgress,
  InputAdornment
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';
import { Products } from '../../Models/Product';
import ProductForm from '../ProductForm/ProductForm';

interface ProductsManagerProps {
  supplierId: number;
  role: 'SUPPLIER' | 'MANAGER'; 
}

const ProductsManager: FC<ProductsManagerProps> = ({ supplierId, role }) => {
  const [products, setProducts] = useState<Products[]>([]);
  const [openForm, setOpenForm] = useState(false);
  const [editProduct, setEditProduct] = useState<Products | undefined>(undefined);
  const [loading, setLoading] = useState(true);
  const [orderQuantities, setOrderQuantities] = useState<{ [productId: number]: number }>({});
  const [orderSuccess, setOrderSuccess] = useState<{ [productId: number]: boolean }>({});
  const [loadingProductIds, setLoadingProductIds] = useState<number[]>([]); 

  const fetchProducts = async () => {
    setLoading(true);
    try {
      const url =
        role === 'SUPPLIER'
          ? `https://localhost:7022/GroceryManagementSystem/Suppliers/get-product-by-supplier/${supplierId}`
          : 'https://localhost:7022/GroceryManagementSystem/Management/all-products';
      const response = await axios.get<Products[]>(url);
      setProducts(response.data);
    } catch (error) {
      console.error('Error loading products:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, [supplierId, role]);

  const handleDelete = async (productId: number) => {
    if (role !== 'SUPPLIER') return;
    if (!window.confirm('Are you sure you want to delete this product?')) return;
    try {
      await axios.delete(`https://localhost:7022/GroceryManagementSystem/Suppliers/delete/${productId}`);
      alert('Product deleted');
      fetchProducts();
    } catch (error) {
      alert('Error deleting');
      console.error(error);
    }

  };

  const handleEditClick = (product: Products) => {
    if (role !== 'SUPPLIER') return;
    setEditProduct(product);
    setOpenForm(true);
  };

  const handleAddClick = () => {
    if (role !== 'SUPPLIER') return;
    setEditProduct(undefined);
    setOpenForm(true);
  };

  const handleSubmitOrder = async (productId: number) => {
    setLoadingProductIds((prev) => [...prev, productId]);
    setOrderSuccess((prev) => ({ ...prev, [productId]: false }));

    try {
      await sendOrderRequest(productId);
      setOrderSuccess((prev) => ({ ...prev, [productId]: true }));
    } catch (err) {
      console.error('Order failed.', err);
      setOrderSuccess((prev) => ({ ...prev, [productId]: false }));
    } finally {
      setLoadingProductIds((prev) => prev.filter((id) => id !== productId));
    }
  };

  const sendOrderRequest = async (productId: number) => {
    if (role !== 'MANAGER') return;
    const quantity = orderQuantities[productId] ?? products.find(p => p.productId === productId)?.minimumPurchaseQuantity;

    if (!quantity || quantity <= 0) {
      alert('Invalid quantity');
      return;
    }
    setLoading(true);
    try {
      await axios.post('https://localhost:7022/GroceryManagementSystem/Management/create-order', {
        productId: productId,
        quantity: quantity,
      });
      setOrderQuantities((prev) => ({ ...prev, [productId]: 0 }));
    } catch (error) {
      console.error(error);
  setLoading(false);

    }
    setLoading(false);

  };

  return (
    <Box sx={{ mt: 4 }} className="ProductsManager" >
      <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2}>
        {role === 'SUPPLIER' && (
          <Button variant="contained" startIcon={<AddIcon />} onClick={handleAddClick}>
            Add New Product
          </Button>
        )}
      </Stack>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Product Name</TableCell>
              <TableCell>Price</TableCell>
              <TableCell>Minimum Quantity</TableCell>
              <TableCell>Total Price</TableCell>
              <TableCell align="center">Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={4} align="center">
                  <CircularProgress />
                </TableCell>
              </TableRow>
            ) : (
              products.map((product) => (
                <TableRow key={product.productId}>
                  <TableCell>{product.productName}</TableCell>
                  <TableCell>{product.itemPrice} $</TableCell>
                  <TableCell>{product.minimumPurchaseQuantity}</TableCell>
                  <TableCell>{product.itemPrice * (orderQuantities[product.productId] || product.minimumPurchaseQuantity)} $</TableCell>
                  <TableCell align="center">
                    {role === 'SUPPLIER' && (
                      <>
                        <IconButton color="primary" onClick={() => handleEditClick(product)}>
                          <EditIcon />
                        </IconButton>
                        <IconButton color="error" onClick={() => handleDelete(product.productId)}>
                          <DeleteIcon />
                        </IconButton>
                      </>
                    )}
                    {role === 'MANAGER' && (
                      <>
                        <TextField
                          className="order-quantity"
                          label="Order Quantity"
                          type="number"
                          value={orderQuantities[product.productId] || product.minimumPurchaseQuantity}
                          onChange={(e) => {
                            const val = Number(e.target.value);
                            if (!isNaN(val) && val >= product.minimumPurchaseQuantity) {
                              setOrderQuantities((prev) => ({
                                ...prev,
                                [product.productId]: val,
                              }));
                            }
                          }}
                          slotProps={{
                            input: {
                              startAdornment: (
                                <InputAdornment position="start">
                                  <IconButton
                                    onClick={() => {
                                      const currentQuantity = orderQuantities[product.productId] || product.minimumPurchaseQuantity;
                                      if (currentQuantity > product.minimumPurchaseQuantity) {
                                        setOrderQuantities((prev) => ({
                                          ...prev,
                                          [product.productId]: currentQuantity - 1,
                                        }));
                                      }
                                    }}
                                  >
                                    -
                                  </IconButton>
                                </InputAdornment>
                              ),
                              endAdornment: (
                                <InputAdornment position="end">
                                  <IconButton
                                    onClick={() => {
                                      const currentQuantity = orderQuantities[product.productId] || product.minimumPurchaseQuantity;
                                      setOrderQuantities((prev) => ({
                                        ...prev,
                                        [product.productId]: currentQuantity + 1,
                                      }));
                                    }}
                                  >
                                    +
                                  </IconButton>
                                </InputAdornment>
                              ),
                            }
                          }}
                          sx={{ width: 160 }}
                        />
                        <Button
                          variant="contained"
                          color={orderSuccess[product.productId] ? 'success' : 'primary'}
                          onClick={() => handleSubmitOrder(product.productId)}
                          disabled={loadingProductIds.includes(product.productId)}
                          className="order-button"
                        >
                          {orderSuccess[product.productId] ? 'Order sent ✔️' : 'Order now'}
                        </Button>
                      </>
                    )}
                  </TableCell>
                </TableRow>
              ))
            )}
            {products.length === 0 && !loading && (
              <TableRow>
                <TableCell colSpan={4} align="center">
                  No products to display.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>

      {role === 'SUPPLIER' && (
        <ProductForm
          open={openForm}
          onClose={() => setOpenForm(false)}
          onSuccess={fetchProducts}
          supplierId={supplierId}
          initialValues={editProduct}
        />
      )}
    </Box>
  );
};

export default ProductsManager;
