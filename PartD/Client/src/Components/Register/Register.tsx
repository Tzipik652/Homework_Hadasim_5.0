import React, { FC, useState } from "react";
import { useFormik } from "formik";
import * as Yup from "yup";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { Box, Button, TextField, Typography, Alert, Stack, TableRow, TableCell, CircularProgress } from "@mui/material";
import { RegisterRequest } from "../../Models/Auth";

const Register: FC = () => {
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [loading, setLoading] = useState(false);
  const [registrationCompleted, setRegistrationCompleted] = useState(false);

  const navigate = useNavigate();

  const formik = useFormik<RegisterRequest>({
    initialValues: {
      Username: "",
      Password: "",
      SupplierDetails: {
        CompanyName: "",
        RepresentativeName: "",
        PhoneNumber: ""
      }
    },
    onSubmit: async (values, { setSubmitting }) => {
      setLoading(true);

      try {
        await axios.post("https://localhost:7022/GroceryManagementSystem/Auth/register", values, {
          headers: { "Content-Type": "application/json" },
        });
        setRegistrationCompleted(true)
        navigate("/login");
      } catch (error) {
        setErrorMessage("Failed to register. Please try again.");
        console.error("Error during registration:", error);
        setLoading(false);

      } finally {
        setLoading(false);

        setSubmitting(false);
      }
    },
    validationSchema: Yup.object({
      Username: Yup.string().required("Username is required."),
      Password: Yup.string().required("Password is required."),
      SupplierDetails: Yup.object({
        CompanyName: Yup.string().required("Company name is required."),
        RepresentativeName: Yup.string().required("Representative name is required."),
        PhoneNumber: Yup.string()
          .matches(/^\+?\d{9,15}$/, "Invalid phone number format")
          .required("Phone number is required."),
      }),
    }),
  });

  return (
    <Box className="auth" maxWidth={400} mx="auto" mt={5} p={4} borderRadius={2} boxShadow={3} bgcolor="white">
      <Typography variant="h5" mb={2} textAlign="center">
        Register
      </Typography>

      <form onSubmit={formik.handleSubmit}>
        <Stack spacing={2}>
          <TextField
            fullWidth
            label="Username"
            name="Username"
            value={formik.values.Username}
            onChange={formik.handleChange}
            error={!!formik.errors.Username}
            helperText={formik.errors.Username}
          />

          <TextField
            fullWidth
            label="Password"
            name="Password"
            type="password"
            value={formik.values.Password}
            onChange={formik.handleChange}
            error={!!formik.errors.Password}
            helperText={formik.errors.Password}
          />

          <TextField
            fullWidth
            label="Company Name"
            name="SupplierDetails.CompanyName"
            value={formik.values.SupplierDetails.CompanyName}
            onChange={formik.handleChange}
            error={!!formik.errors.SupplierDetails?.CompanyName}
            helperText={formik.errors.SupplierDetails?.CompanyName}
          />

          <TextField
            fullWidth
            label="Representative Name"
            name="SupplierDetails.RepresentativeName"
            value={formik.values.SupplierDetails.RepresentativeName}
            onChange={formik.handleChange}
            error={!!formik.errors.SupplierDetails?.RepresentativeName}
            helperText={formik.errors.SupplierDetails?.RepresentativeName}
          />

          <TextField
            fullWidth
            label="Phone Number"
            name="SupplierDetails.PhoneNumber"
            value={formik.values.SupplierDetails.PhoneNumber}
            onChange={formik.handleChange}
            error={!!formik.errors.SupplierDetails?.PhoneNumber}
            helperText={formik.errors.SupplierDetails?.PhoneNumber}
          />

          {errorMessage && <Alert severity="error">{errorMessage}</Alert>}

          <Button type="submit" variant="contained" color="primary" fullWidth>
            Register
          </Button>
          {loading ?
            <TableRow>
              <TableCell colSpan={4} align="center">
                <CircularProgress />
              </TableCell>
            </TableRow>
            : ""}
        </Stack>
      </form>
      {registrationCompleted ?
        <Typography variant="body2" textAlign="center">
          Registration successful. You can now login.{" "}
          <Button variant="text" onClick={() => navigate("/login")}>
            Login here</Button>
            </Typography>
          :
          <Typography variant="body2" textAlign="center">
          Already have an account? {" "}
          <Button variant="text" onClick={() => navigate("/login")}>
            Login here</Button>
            </Typography>
         }
        </Box>
  );
};

      export default Register;
