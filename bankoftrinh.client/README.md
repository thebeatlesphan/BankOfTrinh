# bankoftrinh.client

The frontend application for BankOfTrinh, a modern banking simulator built with Angular 22.2.0 for learning and portfolio practice.

The frontend provides a polished user interface for interacting with the BankOfTrinh backend API. It is being used to practice Angular application structure, standalone components, routing, reactive forms, validation, HTTP communication, state management, responsive design, accessibility, and frontend testing.

The frontend is being developed incrementally by completing one full user journey at a time.

## Current Status

The frontend currently includes the initial application shell and the first routed feature page.

The application shell includes:

- BankOfTrinh branding
- Utility navigation
- Main navigation
- Router outlet for displaying routed pages
- Navigation links for Dashboard, Customers, Accounts, Activity, Help, and Profile

The `/accounts` route is implemented and displays the selected customer's accounts.

The Accounts page currently:

- Loads account data from the backend API
- Displays a loading state while accounts are being requested
- Displays an error message if the request fails
- Displays an empty state when no accounts are returned
- Renders one account card for each returned account
- Displays the account number
- Displays the current balance
- Displays the account creation date

The frontend uses Angular 22.2.0 standalone components. The application does not currently use feature modules.

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
- Backend-connected account loading
- Loading state
- Error state
- Empty state
- Account list rendering
- Responsive heading sizing using CSS `clamp()`
- BankOfTrinh visual language using warm neutrals, charcoal typography, and muted sage accents
- Temporary customer ID used during development

### Account Card

- Standalone `AccountsCardComponent`
- Account card rendered from an account input
- Account number display
- Current balance display
- Account creation date display
- Currency formatting through Angular's `CurrencyPipe`
- Date formatting through Angular's `DatePipe`
- Responsive account card styling

### Accounts API Service

- `AccountsApiService`
- HTTP communication through Angular's `HttpClient`
- Customer account retrieval through:

```text
GET /api/customers/{customerId}/bank-accounts
```

## Project Structure

```text
bankoftrinh.client/
├── src/
│   ├── app/
│   │   ├── features/
│   │   │   └── accounts/
│   │   │       ├── components/
│   │   │       │   └── accounts-card/
│   │   │       │       ├── accounts-card.component.ts
│   │   │       │       ├── accounts-card.component.html
│   │   │       │       └── accounts-card.component.css
│   │   │       ├── pages/
│   │   │       │   └── accounts-page/
│   │   │       │       ├── accounts-page.component.ts
│   │   │       │       ├── accounts-page.component.html
│   │   │       │       ├── accounts-page.component.css
│   │   │       │       └── accounts-page.component.spec.ts
│   │   │       └── services/
│   │   │           └── accounts-api.service.ts
│   │   ├── core/
│   │   │   └── models/
│   │   │       ├── account.model.ts
│   │   │       └── customer-accounts-response.model.ts
│   │   ├── app.routes.ts
│   │   ├── app.config.ts
│   │   ├── app.component.ts
│   │   ├── app.component.html
│   │   ├── app.component.css
│   │   └── app.component.spec.ts
│   │
│   ├── assets/
│   ├── environments/
│   ├── index.html
│   ├── main.ts
│   └── styles.css
│
├── angular.json
├── aspnetcore-https.js
├── karma.conf.js
├── package.json
├── tsconfig.json
└── README.md
```
