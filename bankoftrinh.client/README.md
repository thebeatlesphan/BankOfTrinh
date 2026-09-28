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

### Design Concept

The visual direction is based on a calm, premium banking dashboard:

- Deep navy application shell
- Warm off-white page backgrounds
- White or lightly tinted content cards
- Teal or emerald accent color for positive financial actions
- Strong typographic hierarchy
- Generous spacing
- Rounded but restrained card corners
- Subtle borders and shadows
- Clear success, warning, and error states
- Minimal decorative elements

The application should feel closer to a modern financial dashboard than a generic Bootstrap admin template.

### Design Personality

The interface should be:

- Calm rather than flashy
- Premium rather than overly decorative
- Friendly rather than corporate
- Structured rather than dense
- Functional without feeling plain

Avoid:

- Excessive gradients
- Bright saturated colors everywhere
- Heavy shadows
- Crowded tables
- Too many cards on one screen
- Decorative animations that distract from financial information
- Unnecessary charts before the core workflows are complete

## Proposed Color System

The color palette should use CSS custom properties so that colors can be changed consistently across the application.

```css
:root {
  --color-brand-950: #0b1f33;
  --color-brand-900: #102a43;
  --color-brand-800: #163a5c;
  --color-brand-700: #1f567d;

  --color-accent-600: #087f73;
  --color-accent-500: #0f9d8f;
  --color-accent-100: #d9f3ef;

  --color-page: #f5f7f9;
  --color-surface: #ffffff;
  --color-surface-muted: #eef2f5;

  --color-text-primary: #17212b;
  --color-text-secondary: #5d6b78;
  --color-text-muted: #8a98a6;
  --color-text-inverse: #ffffff;

  --color-border: #dce3e8;
  --color-border-strong: #c4d0d8;

  --color-success-600: #16805d;
  --color-success-100: #ddf5e9;

  --color-warning-600: #a86b00;
  --color-warning-100: #fff1cc;

  --color-danger-600: #c23b4a;
  --color-danger-100: #fde4e7;

  --shadow-sm: 0 1px 2px rgb(16 42 67 / 0.06);
  --shadow-md: 0 8px 24px rgb(16 42 67 / 0.08);

  --radius-sm: 0.375rem;
  --radius-md: 0.625rem;
  --radius-lg: 1rem;
}


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
│   │   └── app.component.ts
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
