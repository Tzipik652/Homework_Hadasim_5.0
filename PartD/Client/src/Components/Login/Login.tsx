import React, { FC, useState } from "react";
import { useFormik } from "formik";
import * as Yup from "yup";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { Box, Button, TextField, Typography, Alert, Stack, TableRow, TableCell, CircularProgress } from "@mui/material";
import { LoginRequest, LoginResponse } from "../../Models/Auth";

interface LoginProps {
  onLoginSuccess: (role: string, id: number) => void;
}

const Login: FC<LoginProps> = ({ onLoginSuccess }) => {
  const [errorMessage, setErrorMessage] = useState<string>("");
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const formik = useFormik<LoginRequest>({
    initialValues: {
      Username: "",
      Password: ""
    },
    onSubmit: async (values, { setSubmitting }) => {
      setLoading(true);

      try {
        const response = await axios.post<LoginResponse>(
          "https://localhost:7022/GroceryManagementSystem/Auth/login",
          values,
          {
            headers: { "Content-Type": "application/json" },
          }
        );

        if (response.data && response.data.role && response.data.id) {
          const { role, id } = response.data;
          console.log("Redirecting to:", role, id); 
          onLoginSuccess(role, id);
          if (role === "SUPPLIER") {
            navigate(`/SUPPLIER/${id}`);
          } else if (role === "MANAGER") {
            navigate(`/MANAGER/${id}`);
          }
        } else {
          setErrorMessage("Invalid login response.");
        }
      } catch (error) {
        setErrorMessage("Invalid username or password.");
        console.error("Error during login:", error);

      } finally {
        setSubmitting(false);
        setLoading(false);

      }
    },

    validationSchema: Yup.object({
      Username: Yup.string().required("Username is required."),
      Password: Yup.string().required("Password is required.")
    })
  });

  return (
    <Box className="auth" maxWidth={400} mx="auto" mt={5} p={4} borderRadius={2} boxShadow={3} bgcolor="white">
      <Typography variant="h5" mb={2} textAlign="center">
        Login
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

          {errorMessage && <Alert severity="error">{errorMessage}</Alert>}

          <Button type="submit" variant="contained" color="primary" fullWidth>
            Login
          </Button>

          <Typography variant="body2" textAlign="center">
            Don't have an account?{" "}
            <Button variant="text" onClick={() => navigate("/Register")}>
              Register here
            </Button>
          </Typography>
          {loading ?
            <Typography variant="body2" textAlign="center">
              <CircularProgress />
            </Typography>
            : ""}
        </Stack>
      </form>
    </Box>
  );
};

export default Login;
