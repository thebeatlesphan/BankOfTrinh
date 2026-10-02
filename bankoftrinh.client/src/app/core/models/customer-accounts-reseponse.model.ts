import { BankAccount } from './account.model';

export interface CustomerAccountsResponse {
  customerId: string,
  accounts: BankAccount[];
}
