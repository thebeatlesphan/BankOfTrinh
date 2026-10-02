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
- Navigation links prepared for future Dashboard, Customers, Transactions, Help, and Profile features

### Accounts Page

- `AccountsPageComponent`
- Accounts page HTML template
- Accounts page component styling
- Initial BankOfTrinh visual language using warm neutrals, charcoal typography, and muted sage accents
- Responsive heading sizing using CSS `clamp()`

## Planned Features

The frontend will eventually support:

- Customer browsing and management
- Bank account browsing
- Account balance summaries
- Account transaction history
- Deposits
- Reactive deposit forms
- Client-side validation
- Backend API communication
- Loading states
- Error states
- Shared account and transaction models
- Angular services for backend communication
- Responsive layouts
- Accessibility improvements
- Component and service testing

## Visual Direction

BankOfTrinh should feel like a calm, thoughtful financial workspace rather than a traditional banking portal or generic admin dashboard.

The visual language uses warm neutrals, charcoal typography, muted sage accents, and restrained coral highlights. The interface should feel trustworthy and structured while remaining approachable and distinctive.

### Design Personality

The interface should be:

- Grounded rather than flashy
- Human rather than corporate
- Clear rather than dense
- Warm rather than cold
- Confident rather than aggressive

Avoid:

- Heavy navy application shells
- Generic Bootstrap dashboard styling
- Excessive gradients
- Large decorative illustrations
- Dense data tables
- Overuse of accent colors
- Unnecessary animations

## Project Structure

```text
bankoftrinh.client/
├── src/
│   ├── app/
│   │   ├── features/
│   │   │   └── accounts/
│   │   │       └── pages/
│   │   │           └── accounts-page/
│   │   │               ├── accounts-page.component.ts
│   │   │               ├── accounts-page.component.html
│   │   │               ├── accounts-page.component.css
│   │   │               └── accounts-page.component.spec.ts
│   │   │
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

Create a static AccountCardComponent and display one hard-coded account card inside AccountsPageComponent before connecting the frontend to the backend API.
