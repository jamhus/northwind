import React from "react";

type CardProps = {
  className?: string;
  children: React.ReactNode;
};

export function Card({ className = "", children }: CardProps) {
  return (
    <div className={`rounded-lg border border-gray-200 bg-white shadow-sm ${className}`}>
      {children}
    </div>
  );
}

type CardContentProps = {
  className?: string;
  children: React.ReactNode;
};

export function CardContent({ className = "", children }: CardContentProps) {
  return <div className={`p-4 ${className}`}>{children}</div>;
}
