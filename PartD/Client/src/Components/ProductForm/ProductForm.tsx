import React, { FC, useEffect } from 'react';
import './ProductForm.scss';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button
} from '@mui/material';
import axios from 'axios';
import { Products, NewProduct } from '../../Models/Product';

interface ProductFormProps {
  open: boolean;
  onClose: () => void;
  onSuccess: () => void;
  supplierId: number;
  initialValues?: Products;
}

const ProductForm: FC<ProductFormProps> = ({ open, onClose, onSuccess, supplierId, initialValues }) => {
  const isEdit = !!initialValues;

  const formik = useFormik<NewProduct>({
    initialValues: {
      SupplierId: supplierId,
      ProductName: isEdit ? initialValues?.productName || '' : '',
      ItemPrice: isEdit ? initialValues?.itemPrice || 0 : 0,
      MinimumPurchaseQuantity: isEdit ? initialValues?.minimumPurchaseQuantity || 1 : 1,
    },

    validationSchema: Yup.object({
      ProductName: Yup.string().required("Enter a product name"),
      ItemPrice: Yup.number().positive("Price must be positive").required("Enter a price"),
      MinimumPurchaseQuantity: Yup.number().integer().positive("Quantity must be positive").required("Enter a minimum quantity"),
    }),

    enableReinitialize: true,

    onSubmit: async (values, { setSubmitting }) => {
      try {
        if (isEdit) {
          await axios.put("https://localhost:7022/GroceryManagementSystem/Suppliers/update", {
            ...initialValues,
            ...values,
          });
        } else {
          await axios.post("https://localhost:7022/GroceryManagementSystem/Suppliers/create", values);
        }

        onSuccess();
        onClose();
      } catch (err) {
        alert("Error submitting form");
        console.error(err);
      } finally {
        setSubmitting(false);
      }
    }
  });

  useEffect(() => {
    if (!isEdit && open) {
      formik.resetForm({
        values: {
          SupplierId: supplierId,
          ProductName: '',
          ItemPrice: 0,
          MinimumPurchaseQuantity: 1,
        }
      });
    }
  }, [open, isEdit]);

  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>{isEdit ? 'Edit Product' : 'Add New Product'}</DialogTitle>
      <form onSubmit={formik.handleSubmit}>
        <DialogContent sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
          <TextField
            name="ProductName"
            label="product name"
            value={formik.values.ProductName}
            onChange={formik.handleChange}
            error={!!formik.errors.ProductName}
            helperText={formik.errors.ProductName}
            fullWidth
          />

          <TextField
            name="ItemPrice"
            label="price per unit"
            type="number"
            value={formik.values.ItemPrice}
            onChange={formik.handleChange}
            error={!!formik.errors.ItemPrice}
            helperText={formik.errors.ItemPrice}
            fullWidth
          />

          <TextField
            name="MinimumPurchaseQuantity"
            label="minimum quantity"
            type="number"
            value={formik.values.MinimumPurchaseQuantity}
            onChange={formik.handleChange}
            error={!!formik.errors.MinimumPurchaseQuantity}
            helperText={formik.errors.MinimumPurchaseQuantity}
            fullWidth
          />
        </DialogContent>

        <DialogActions>
          <Button onClick={onClose}>Cancel</Button>
          <Button type="submit" variant="contained" color="primary">
            {isEdit ? 'update' : 'add'}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
};

export default ProductForm;