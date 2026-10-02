import { Routes } from '@angular/router';
import { AccountsPageComponent } from './features/accounts/pages/accounts-page/accounts-page.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'accounts',
    pathMatch: 'full'
  },
  {
    path: 'accounts',
    component: AccountsPageComponent
  }
];
