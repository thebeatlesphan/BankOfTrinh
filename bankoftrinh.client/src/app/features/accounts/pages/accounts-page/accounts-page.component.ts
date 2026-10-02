import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { BankAccount } from '../../../../core/models/account.model';
import { AccountCardComponent } from '../../components/account-card/account-card.component';

@Component({
  selector: 'app-accounts-page',
  standalone: true,
  imports: [AccountCardComponent],
  templateUrl: './accounts-page.component.html',
  styleUrl: './accounts-page.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AccountsPageComponent {
  readonly account: BankAccount = {
    id: 'a2f3b8e3-64b2-4bb5-b0f4-08dc12345678',
    customerId: 'f4b8d2a1-4a89-4c4e-9d2b-08dc12345678',
    accountNumber: '4827361950',
    balance: 4280.75,
    createdAtUtc: '2026-09-15T14:30:00Z',
  };
}
