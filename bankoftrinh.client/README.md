# bankoftrinh.client

The frontend application for BankOfTrinh, a modern banking simulator built with Angular for learning and portfolio practice.

The frontend will provide a polished user interface for interacting with the BankOfTrinh backend API. It will be used to practice Angular application structure, components, services, routing, reactive forms, validation, HTTP communication, state management, responsive design, accessibility, and frontend testing.

## Current Status

The Angular frontend has not been implemented yet.

The backend currently provides functionality for:

- Customer management
- Bank account management
- Deposits
- Account balance updates
- Persisted account transactions
- Request validation
- Integration testing

The frontend will be developed incrementally by completing one full user journey at a time.

## Current Focus

The current frontend focus is defining the visual foundation and implementing customer creation.

## Product Vision

BankOfTrinh should feel like a small, trustworthy digital banking application rather than a collection of disconnected demo screens.

The interface should communicate:

- Trust
- Clarity
- Stability
- Simplicity
- Professionalism
- Financial confidence

The design should be modern and polished without looking like a copy of a real bank. It should remain understandable as a learning project and avoid unnecessary visual complexity.

## Visual Direction

BankOfTrinh should feel like a calm, thoughtful financial workspace rather
than a traditional banking portal or generic admin dashboard.

The visual language uses warm neutrals, charcoal typography, muted sage accents,
and restrained coral highlights. The interface should feel trustworthy and
structured while remaining approachable and distinctive.

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

## Recommended Project Structure

```text
bankoftrinh.client/
├── src/
│   ├── app/
│   │   ├── core/
│   │   │   ├── errors/
│   │   │   ├── interceptors/
│   │   │   ├── models/
│   │   │   └── services/
│   │   │
│   │   ├── shared/
│   │   │   ├── components/
│   │   │   ├── directives/
│   │   │   └── pipes/
│   │   │
│   │   ├── features/
│   │   │   ├── customers/
│   │   │   │   ├── create-customer/
│   │   │   │   ├── get-customer/
│   │   │   │   └── customer.models.ts
│   │   │   │
│   │   │   ├── accounts/
│   │   │   │   ├── create-account/
│   │   │   │   ├── account-details/
│   │   │   │   ├── customer-accounts/
│   │   │   │   └── account.models.ts
│   │   │   │
│   │   │   └── transactions/
│   │   │       ├── deposit/
│   │   │       ├── withdrawal/
│   │   │       ├── transaction-history/
│   │   │       └── transaction.models.ts
│   │   │
│   │   ├── app.routes.ts
│   │   ├── app.config.ts
│   │   └── app.component.html
│   │   └── app.component.css
│   │   └── app.component.ts
│   │   └── app.component.spec.ts
│   │   └── app.module.ts
│   │
│   ├── assets/
│   ├── environments/
│   ├── index.html
│   ├── main.ts
│   └── styles.css
│
├── angular.json
├── package.json
├── tsconfig.json
└── README.md

```
