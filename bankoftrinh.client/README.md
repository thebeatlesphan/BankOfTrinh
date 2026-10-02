# bankoftrinh.client

The frontend application for BankOfTrinh, a modern banking simulator built with Angular 22.2.0 for learning and portfolio practice.

The frontend will provide a polished user interface for interacting with the BankOfTrinh backend API. It is being used to practice Angular application structure, standalone components, routing, reactive forms, validation, HTTP communication, state management, responsive design, accessibility, and frontend testing.

The frontend is being developed incrementally by completing one full user journey at a time.

## Current Status

The frontend currently includes the initial application shell and the first routed feature page.

The application shell includes:

- BankOfTrinh branding
- Utility navigation
- Main navigation
- Router outlet for displaying routed pages
- Navigation links for Dashboard, Customers, Accounts, Activity, Help, and Profile

The `/accounts` route is currently implemented and displays an initial Accounts page with:

- BankOfTrinh eyebrow label
- “Your accounts” page heading
- Introductory account activity text
- One static account card
- Account number display
- Current balance display
- Account creation date

The frontend uses Angular 22.2.0 standalone components. The application does not currently use feature modules for the Accounts page.

## Implemented Features

### Application Shell

- Angular standalone component architecture
- Root `AppComponent`
- Shared application shell
- BankOfTrinh brand navigation
- Utility navigation
- Main navigation
- Active navigation link styling through `RouterLinkActive`
- Routed content rendered through `<router-outlet />`

### Routing

- Root route redirects to `/accounts`
- `/accounts` route
- `AccountsPageComponent` routed through `app.routes.ts`
- Navigation links prepared for future Dashboard, Customers, Activity, Help, and Profile features

### Accounts Page

- `AccountsPageComponent`
- Accounts page HTML template
- Accounts page component styling
- Initial BankOfTrinh visual language using warm neutrals, charcoal typography, and muted sage accents
- Responsive heading sizing using CSS `clamp()`
- Static account data matching the current backend account shape

### Account Card

- Standalone `AccountCardComponent`
- Account card rendered from an account input
- Account number display
- Current balance display
- Account creation date display
- Currency formatting through Angular's `CurrencyPipe`
- Date formatting through Angular's `DatePipe`
- Responsive account card styling

### Frontend Account Model

The current frontend account model reflects the backend `BankAccount` shape:

```ts
export interface BankAccount {
  id: string;
  customerId: string;
  accountNumber: string;
  balance: number;
  createdAtUtc: string;
}
```

Recommended next step is to connect the Accounts page to the backend API.
