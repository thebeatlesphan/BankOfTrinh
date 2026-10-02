import { ChangeDetectionStrategy, Component, inject, Input, signal } from '@angular/core';
import { BankAccount } from '../../../../core/models/account.model';
import { AccountCardComponent } from '../../components/account-card/account-card.component';
import { AccountsApiService } from '../../services/accounts-api.service';

@Component({
  selector: 'app-accounts-page',
  standalone: true,
  imports: [AccountCardComponent],
  templateUrl: './accounts-page.component.html',
  styleUrl: './accounts-page.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AccountsPageComponent {
  private readonly accountsApi = inject(AccountsApiService);

  readonly accounts = signal<BankAccount[]>([]);
  readonly isLoading = signal(true);
  readonly errorMessage = signal<string | null>(null);

  private readonly customerId = '680F04B8-80AA-449E-A812-B382E7548652';

  constructor() {
    this.loadAccounts();
  }

  private loadAccounts(): void {
    this.accountsApi.getCustomerAccounts(this.customerId).subscribe({
      next: (accounts) => {
        this.accounts.set(accounts);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set("Unable to load your accounts.");
        this.isLoading.set(false);
      },
    });
  }
}
