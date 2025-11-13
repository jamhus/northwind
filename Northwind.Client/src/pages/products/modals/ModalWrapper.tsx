import Modal from "react-modal";
import { X } from "lucide-react";
import type { ReactNode } from "react";
import { motion, AnimatePresence } from "framer-motion";

type ModalWrapperProps = {
  isOpen: boolean;
  onClose: () => void;
  children: ReactNode;
  className?: string;
  width?: string;
  title?: string;
};

export default function ModalWrapper({
  isOpen,
  onClose,
  children,
  className = "",
  width = "w-2xl",
  title,
}: ModalWrapperProps) {
  return (
    <AnimatePresence>
      {isOpen && (
        <Modal
          isOpen={isOpen}
          onRequestClose={onClose}
          shouldCloseOnOverlayClick={true}
          overlayClassName="fixed inset-0 bg-black/20 flex justify-center items-center z-40"
          className={`bg-transparent outline-none ${width} border-none flex justify-center items-center`}
        >
          {/* Overlay-animation */}
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.15 }}
            className="fixed inset-0 bg-black/20"
          />

          {/* Själva modalen */}
          <motion.div
            initial={{ opacity: 0, scale: 0.95, y: 10 }}
            animate={{ opacity: 1, scale: 1, y: 0 }}
            exit={{ opacity: 0, scale: 0.97, y: 10 }}
            transition={{ duration: 0.15, ease: "easeOut" }}
            className={`bg-white rounded-xl p-6 ${width} shadow-xl relative z-50 ${className}`}
          >
            {title && (
              <div className="flex justify-between items-center mb-4">
                <h2 className="text-xl font-semibold">{title}</h2>
                <button
                  onClick={onClose}
                  className="text-gray-500 hover:text-gray-800"
                >
                  <X size={20} />
                </button>
              </div>
            )}{" "}
            {children}
          </motion.div>
        </Modal>
      )}
    </AnimatePresence>
  );
}
