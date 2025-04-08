import React, { useEffect, useState } from 'react';
import { Container, Typography, AppBar, Toolbar, Box, Button } from '@mui/material';
import { BrowserRouter, Routes, Route, Navigate, useLocation } from 'react-router-dom';

import Dashboard from './Components/Dashboard/Dashboard';
import Login from './Components/Login/Login';
import Register from './Components/Register/Register';

const App: React.FC = () => {
  const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);
  const [userRole, setUserRole] = useState<string | null>(null);
  const [userId, setUserId] = useState<number | -1>(-1);
  const [loading, setLoading] = useState(true);
  const onLoginSuccess = (role: string, id: number) => {
    setUserRole(role);  
    setUserId(id);
    setIsLoggedIn(true);
  };
  
  useEffect(() => {
    const role = localStorage.getItem("role");
    const id = localStorage.getItem("id");
    if ((role === "SUPPLIER" || role === "MANAGER") && !isNaN(Number(id))) {
      if (role && id) {
        setUserRole(role);
        setUserId(Number(id));
        setIsLoggedIn(true);
      }
    } else {
      localStorage.removeItem("role");
      localStorage.removeItem("id");
    }
    setLoading(false);
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("role");
    localStorage.removeItem("id");
    setUserRole(null);
    setUserId(-1);
    setIsLoggedIn(false);
  };

  const ProtectedRoute = ({ children }: { children: React.ReactNode }) => {
    if (!isLoggedIn) return <Navigate to="/" />;
    return <>{children}</>;
  };

  return (
    <BrowserRouter>
      <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
        <AppBar position="static">
          <Toolbar>
            <Typography variant="h6">Grocery Management System</Typography>
            {isLoggedIn && (
              <Button color="inherit" onClick={handleLogout} sx={{ marginLeft: 'auto' }}>
                Logout
              </Button>
            )}
          </Toolbar>
        </AppBar>

        <Container sx={{ flexGrow: 1, marginTop: '2rem' }}>
          <Routes>
            <Route
              path="/"
              element={
                loading ? (
                  <div>Loading...</div>
                ) : isLoggedIn ? (
                  userRole === "SUPPLIER" ? (
                    <Navigate to={`/SUPPLIER/${userId}`} />
                  ) : userRole === "MANAGER" ? (
                    <Navigate to={`/MANAGER/${userId}`} />
                  ) : null
                ) : (
                  <Login onLoginSuccess={(role: string, id: number) => {
                    setUserRole(role);
                    setUserId(id);
                    setIsLoggedIn(true);
                  }} />
                )
              }
            />

            <Route
              path="/SUPPLIER/:id/*"
              element={
                <ProtectedRoute>
                  <Dashboard role={'SUPPLIER'} supplierId={userId} />
                </ProtectedRoute>
              }
            />
            <Route
              path="/MANAGER/:id/*"
              element={
                <ProtectedRoute>
                  <Dashboard role={'MANAGER'} supplierId={userId} />
                </ProtectedRoute>
              }
            />
            <Route
              path="/Register"
              element={<Register />}
            />
            <Route
              path="/Login"
              element={
                <Login onLoginSuccess={onLoginSuccess} />

              }
            />
          </Routes>
        </Container>

        <Box
          sx={{
            flexShrink: 0,
            textAlign: 'center',
            padding: 2,
            background: '#f5f5f5',
            borderTop: '1px solid #ddd',
          }}
        >
          <Typography variant="body2" color="textSecondary">
            All rights reserved &copy; 2025 | Developed by Tzipora Kroizer
          </Typography>
        </Box>
      </Box>
    </BrowserRouter>
  );
};

export default App;
