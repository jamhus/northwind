import './index.css'
import App from './App.tsx'
import Modal from "react-modal";
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Toaster } from 'react-hot-toast';


Modal.setAppElement("#root");

const queryClient = new QueryClient();

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <App />
      <Toaster
        position="bottom-right"
        toastOptions={{
          duration: 3000,
          style: { fontSize: "0.9rem" },
          success: { iconTheme: { primary: "#10B981", secondary: "white" } },
          error: { iconTheme: { primary: "#EF4444", secondary: "white" } },
      }}
    />
    </QueryClientProvider>
  </StrictMode>,
)
