export interface LoginRequest {
    Username: string;
    Password: string;
  }
  
export interface RegisterRequest {
    Username: string;
    Password: string;
    SupplierDetails: {
      CompanyName: string;
      RepresentativeName: string;
      PhoneNumber: string;
    };
  }
  export interface LoginResponse {
    role: string;
    id: number;
  }