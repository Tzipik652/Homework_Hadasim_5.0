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
 

  const handleLogout = () => {
    setIsLoggedIn(false);
  };

  const ProtectedRoute = ({ children }: { children: React.ReactNode }) => {
    if (!isLoggedIn) return <Navigate to="/" />;
    return <>{children}</>;
  };

  return (
    <BrowserRouter basename="/grocery-management-system">
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
                isLoggedIn ? (
                  userRole === "SUPPLIER" ? (
                    <Navigate to={`/supplier`} state={{userId,userRole}}/>
                  ) : userRole === "MANAGER" ? (
                    <Navigate to={`/manager`} state={{userId,userRole}}/>
                  ) : null
                ) : (
                  <Login onLoginSuccess={ ()=>setIsLoggedIn(true)} />
                )
              }
            />

            <Route
              path="/supplier"
              element={
                <ProtectedRoute>
                  <Dashboard />
                </ProtectedRoute>
              }
            />
            <Route
              path="/manager"
              element={
                <ProtectedRoute>
                  <Dashboard />
                </ProtectedRoute>
              }
            />
            <Route
              path="/register"
              element={<Register />}
            />
            <Route
              path="/login"
              element={
                <Login onLoginSuccess={()=>setIsLoggedIn(true)} />

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
