import { CurrencyPipe, DatePipe } from "@angular/common";
import { ChangeDetectionStrategy, Component, Input } from "@angular/core";
import { BankAccount } from "../../../../core/models/account.model";

@Component({
  selector: 'app-account-card',
  standalone: true,
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './account-card.component.html',
  styleUrl: './account-card.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AccountCardComponent {
  @Input({ required: true}) account!: BankAccount;

  readonly currencyCode = 'USD';
}
