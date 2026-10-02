import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { BankAccount } from "../../../core/models/account.model";
import { map, Observable } from "rxjs";
import { CustomerAccountsResponse } from "../../../core/models/customer-accounts-reseponse.model";

@Injectable({
  providedIn: 'root',
})
export class AccountsApiService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = 'https://localhost:7090/api';

  getCustomerAccounts(customerId: string): Observable<BankAccount[]> {
    return this.http
    .get<CustomerAccountsResponse>(
      `${this.apiUrl}/customers/${customerId}/bank-accounts`,
    )
    .pipe(map((response) => response.accounts));
  }
}
