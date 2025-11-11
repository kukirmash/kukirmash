import React from 'react'
import { LoginPage } from "./pages/LoginPage/LoginPage";
import { RegisterPage } from './pages/RegisterPage/RegisterPage';
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { MainPage } from './pages/MainPage/MainPage';

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/main" element={<MainPage />} />
        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    </BrowserRouter>
  )
}
