/// in future we can make this interface common/general for all  using generic type and dynamic property names
export interface ApiResponse<T = null> {
  status: boolean;
  message: string;
  data: T;
}
