# CLAUDE.md — Mascotas Project

Use this root memory for Mascotas. For business-idea research, evaluation, and ranking, follow [claude-ideas.md](https://github.com/erickvalderrama/it/blob/main/claude-ideas.md).

## Official product name

**Petyei** is the official application name, chosen by the owner on 11 November 2026. Write it as `Petyei` in text and `petyei` in the wordmark. "Mascotas" remains the internal project name in existing documents. Use Petyei in user-facing text, store listings, the domain, and legal documents once the operator data is completed. Brand drafts, palette usage, and pending checks are in [documents/brand/petyei](documents/brand/petyei/) and [documents/brand/README.md](documents/brand/README.md). The `petyei.com` domain was unregistered on 11 November 2026; its standard price, registration, and the trademark search are pending.

## Working role and delivery

Act as a senior software architect, full-stack engineer, UI/UX product designer, DevOps engineer, and QA engineer. Apply expertise in clean architecture, C# and PostgreSQL, Flutter web and mobile, interaction design and usability, CI/CD and operations, and risk-based testing. The owner is experienced in frontend, backend, servers, and infrastructure. Discuss technical decisions and trade-offs at that level.

Read the existing code and approved model before starting. For each change, identify the active phase, affected roles and resource ownership, data and API contracts, UI states, failure and recovery paths, and acceptance checks. Deliver one usable slice through the necessary layers, preserve user changes, and document material decisions and unresolved blockers.

For Phase 1, use the implementation specification for the build order, proposed API and data contracts, UI/UX journeys, and acceptance scenarios. Bring a copy into the application repository at docs/specifications/phase-1.md and use the approved model for persistence. [Phase 1 implementation specification](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/phase-1-specification.md)

### Work plan execution and handoff

Track execution in the application's `docs/planning/work-plan.md`. Read the current checklist, specification, repository instructions, and code before starting. Resume from the first runnable unchecked task in the requested phase. [Work plan template](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/work-plan.md)

Complete the task through its normal entry points, run the relevant checks, mark `[x]` after verification, and record evidence in the plan. Continue through the available tasks in order. Record reversible assumptions and keep blocked tasks unchecked with their dependency. Proceed with independent authorized work. After exhausting runnable tasks, report the next step and group any questions that need the owner's input.

## Project and stack

Build the requested change in small, verifiable increments. Mascotas connects people with adoption foundations, independent pet service providers, and pet supply stores.
Use C# with ASP.NET Core Web API, PostgreSQL, Entity Framework Core, and Npgsql. Connect to the existing external PostgreSQL server.
Use Dockerfiles and Docker Compose for the API and Flutter web. Connect the backend to the owner's external MinIO server for object storage. Inject environment-specific credentials at runtime on the server.

### One frontend and one backend — no microservices

Owner rule, 11 November 2026. Build exactly two deployable applications: one Flutter frontend for web, Android, and iOS, and one ASP.NET Core backend organized as a modular monolith.
- Do not split the system into microservices, separate databases per module, or additional backend services. Do not add a message broker, service mesh, Kubernetes, or a separate search engine unless the owner approves a specific need.
- Run background work such as moderation, invoicing, subscription charges, reconciliation, and model training inside the backend as hosted services backed by persisted jobs. Scale the same backend image when needed.
- Keep module boundaries inside the backend through namespaces, projects, and ports, using one PostgreSQL database.
- Do not use Ethereum or any other blockchain. The thesis proposed blockchain adoption certificates; that part is out of scope. Keep any adoption certificate or record in PostgreSQL and MinIO with audit history.
Use one Flutter and Dart application for web, Android, and iOS. Use Riverpod as the initial state management choice and respect a state solution already approved for the project.
Check existing dependency versions and official compatibility guidance before introducing or upgrading packages.

## Golden rule — Approved entity relationship model

The approved entity relationship model governs persistence. Record its path here when available. Approved model path is pending.

Adoption and matching reference model: the owner approved the thesis ERD for adoptions, pet catalogs, and the matching questionnaires on 11 November 2026. Use [documents/prompts/mascotas/model](documents/prompts/mascotas/model/README.md) and apply its listed adaptations. The remaining areas still need the approved model.
- Match entities, columns, types, precision, nullability, keys, cardinalities, constraints, and deletion rules.
- Derive mappings and migrations from the approved model. Treat the functional modules and role names below as requirements for behavior.
- Use English code identifiers and explicit mappings for physical database names required by the model. Resolve schema naming conflicts with the approved model before generating a migration.
- When the approved model is unavailable for a persistence change, identify the missing input and continue work independent of the schema.
- For a feature that requires an unapproved table, field, relationship, enum value, or change to a constraint, document the exact model change and its impact. Continue independent work and apply a coordinated model, mapping, migration, and test change once authorized.
- Reconcile any discrepancy between the model, migrations, and existing database before altering persistence. Apply real database migrations or destructive operations only within the user's authorized scope.

## Persistence rules — soft delete, indexes, and migrations

Owner rules, 11 November 2026. Apply them to every entity and schema change.

**Logical deletion only.** Never physically delete business records. Deleting a record changes its state in the database.
- Give each deletable entity a status field with a deleted or inactive value, plus `DeletedAt` and `DeletedBy` for audit. Expose deletion as a state transition in the use case, and audit actor, time, and reason.
- Exclude deleted records from normal queries through EF Core global query filters. Allow authorized administrative and audit queries to include them explicitly. Restore a record by changing its state back when the workflow allows it.
- Declare unique constraints as partial unique indexes over non-deleted rows when a deleted record must not block reuse, such as an email address.
- Configure foreign keys with restricted deletes. Do not rely on cascade deletes.
- Handle a personal data deletion request under Colombian data protection law by anonymizing or suppressing the personal fields with an audited state change, while preserving payment, invoice, and legal records required by law. Retire stored objects in MinIO through the same state and retention policy.

**Indexes are required.** Create indexes for every foreign key, the columns used in filters, sorting, joins, and pagination, uniqueness rules, and state columns used by background jobs. Use composite and partial indexes that match the actual queries, including filters on non-deleted rows. Review query plans with representative data for search, admin queues, and job polling.

**Migrations in .NET only.** Change the database schema exclusively through Entity Framework Core migrations in the Infrastructure project. Do not apply manual DDL. Review each generated migration, keep it within the 699-line limit, and deploy with idempotent scripts or migration bundles. Seed catalogs through migrations or versioned import jobs.

## English throughout the codebase

Use English for all authored code, filenames, directories, namespaces, classes, methods, variables, constants, comments, API routes, DTOs, error codes, fixtures, and test names. Use English names in new database models and migration code while preserving approved physical mappings.
Use English as the base and fallback language for interface copy. Keep all copy in localization resources, separate from source code, as described in "Languages and localization". Preserve user-entered content and the product name Petyei.
Never hardcode user-facing text in widgets, controllers, or the API. Reference localized keys such as `context.l10n.checkoutTitle`. Include titles, labels, hints, actions, dialogs, validation messages, notifications, emails, and loading, empty, and error states.
Use templates with parameters for changing values such as prices, dates, and counts. Render business data supplied by the API and map stable API error codes to centralized user messages. Keep environment URLs, secrets, rates, and plan values in their proper configuration or data source.

## Languages and localization — Phase 1

Owner rule, 11 November 2026. Petyei is multilingual by architecture. Phase 1 ships English and Spanish. Adding or maintaining a language must not require code changes beyond registering the locale.
- **Flutter.** Use `flutter_localizations` and `gen-l10n` with ARB files: `app_en.arb` is the template and `app_es.arb` the first translation. Use ICU placeholders, plurals, and selects for counts, prices, and dates. Format dates, numbers, and currency with `intl` for the active locale; keep currency from the data, not from the language.
- **Locale selection.** Detect the device or browser language on first launch, fall back to English for unsupported languages, and let the user change it in account settings and before sign-in. Persist the choice on the device and in the account. Switch language without restarting and keep the current route.
- **Backend.** Return stable error codes and data, not translated sentences, so the client renders the message. Localize server-generated content such as emails, push notifications, and invoice notes from resource files keyed by locale, using the account's language. Accept `Accept-Language` and the stored preference.
- **Translatable data.** Store content managed in the database with per-language translations: species and breeds, questionnaire questions and options, subscription plans and benefits, moderation reasons, notification templates, and legal documents. Use a translation table keyed by entity, field, and locale, or the approved model's equivalent, with fallback to English.
- **Adding a language.** Document the steps: add the ARB file, register the locale, add translations for catalogs and templates, and add the legal documents in that language. A CI check fails on missing or unused keys, placeholder mismatches, and untranslated catalog entries.
- **User content.** Do not translate listings, names, or messages written by users automatically. Store the language when known.
- **Layout.** Reserve space for longer translations, avoid text in images, and keep layouts ready for right-to-left languages through directional padding and alignment.

## Clean architecture and file limit

Organize the backend as a modular monolith.
- Domain owns entities, value objects, and invariants. Keep it independent of persistence, HTTP, and payment providers.
- Application owns use cases, DTOs, validation, and ports. It depends on Domain.
- Infrastructure implements persistence and external integrations. Put EF Core mappings, migrations, and ePayco adapters here.
- API composes dependencies, authorizes requests, translates errors, and invokes use cases. Return API DTOs and keep business decisions in Domain and Application.
- Use asynchronous I/O, cancellation, pagination, and projections where relevant. Keep related state transitions consistent in transactions.
- Keep API contracts compatible with released mobile clients during rollout. Use stable error codes and safe correlation references, and test the contract shared by the API and Flutter.
- Define transitions and allowed actors for each workflow. For payments, invoices, moderation, and other asynchronous work, persist the operation and its progress. Specify idempotency, duplicate or out-of-order messages, bounded retries, and authorized recovery before wiring external calls.

Organize Flutter by feature. Place app startup, routing, and theme in lib/app and truly shared infrastructure and components in lib/core. Keep domain contracts and use cases, data sources and repository implementations, and presentation widgets and controllers separate within each feature.
Widgets render state and send user actions to Riverpod controllers. Controllers invoke use cases and expose the resulting state. Keep business calculations and HTTP access in their own layers.
Keep every source file below 700 lines, including comments, blank lines, widgets, screens, tests, and migrations. The maximum is 699. Split by responsibility and keep formatting readable. Adjust generator inputs when generated code exceeds the limit and report a concrete blocker if the generator prevents compliance. Enforce the limit in project checks.

## UI/UX and Flutter interface for web and mobile

Use **Bosque y pistacho** as the official light palette. Set `primary` to `#2B5735`, `accentContainer` to `#D7EC9E`, `canvas` to `#F5F8EE`, `surface` and `onPrimary` to `#FFFFFF`, and `onSurface` and `onAccentContainer` to `#243329`. Apply the tokens through the shared Flutter theme for web and mobile. Use forest green for primary actions and selected controls and pistachio for featured listings. [Official theme tokens](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/phase-1-specification.md#official-light-theme)

### Dark mode — Phase 1

Owner rule, 11 November 2026. Ship light and dark themes in Phase 1 on web, Android, and iOS.
- Offer System, Light, and Dark in account settings and before sign-in. Default to System. Persist the choice on the device and in the account so it follows the user after sign-in. Apply it without restarting.
- Define both themes from the same semantic tokens in the shared Flutter theme. Widgets consume tokens only; never hardcode colors.
- Proposed dark tokens derived from Bosque y pistacho: `canvas` `#121A14`, `surface` `#1C2620`, `primary` `#B9D88A`, `onPrimary` `#16231A`, `accentContainer` `#2F4A33`, `onAccentContainer` `#D7EC9E`, `onSurface` `#E3ECDF`. Measured contrast ranges from 7.6:1 to 14.6:1. Keep every text and icon pair at WCAG AA 4.5:1 or higher and 3:1 for large text and UI boundaries.
- Provide dark variants of images that need them, including the Petyei logo, icon, maps, charts, and status colors. Status remains readable through text and icons as well as color.
- Cover both themes in golden tests and accessibility checks for every critical screen.

Apply UI/UX experience in information architecture, task flows, accessible interaction, design systems, prototyping, and usability evaluation. Define the user's goal, entry point, primary action, success feedback, and recovery before implementing a critical screen. Sketch compact and wide layouts with representative data and verify that people can complete the task before refining its visual details.
- Reduce steps and defer information until the relevant action. Keep city selection visible, let visitors browse first, preserve context after sign-in, and explain price, review status, and the next action at the point where they matter.
- Evaluate discovery, publication, checkout status, and paid correction with representative pilot users. Record observed problems, fix blockers, and cover the corrected behavior in the relevant tests.

Design each screen once around the same use case and controller. Compose layouts from shared widgets using the available width. Centralize breakpoints, spacing, color, typography, and component states in the design system. Use layout constraints to choose compact, medium, and wide compositions.
- Compact screens use a single readable content column, accessible navigation, and reachable primary actions. Respect safe areas and keep inputs visible when the keyboard opens.
- Wider screens use side navigation, bounded reading widths, or list and detail panels where useful. Make tables and forms usable at narrower widths by wrapping or adapting content and keeping key actions visible.
- Keep navigation and deep links consistent. Handle browser refresh, back and forward navigation, keyboard focus, and tab order. Support touch, mouse, and keyboard where relevant.
- Use consistent components for forms, cards, status, and actions. Show status in text as well as color. Keep the primary action reachable and give icon-only actions accessible labels.
- Verify focus, contrast, text scaling, wrapping, and scroll behavior. Reserve space for long names, translated copy, large amounts, and validation messages.
- Define loading, empty, error, retry, offline, unauthorized, and success states for each data-driven screen. Preserve form data on recoverable errors and display messages next to the affected fields.
- Separate view states from transaction states. Disable repeat submission while a request is pending. Use the backend result to display payment, reservation, subscription, or delivery confirmation.
- Isolate platform capabilities behind adapters. Confirm camera, uploads, location, maps, and payment flows on the supported targets. Show an actionable fallback where a capability is unavailable. Web users can follow a trip and see the last authorized position. Enable location collection in the background on supported mobile devices after validation.
- Define the expected layout and key states of each critical screen in the design system. Review changed screens at compact, medium, and wide widths. Validate navigation and the primary action end to end on web and mobile.

## Release plan and roles

Ship the adoption release first. Use the work plan for dependencies, acceptance checks, release gates, and the Colombia pilot. Follow the approved entity relationship model when implementing these requirements. [Mascotas work plan](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/work-plan.md)

| Role | Scope |
| --- | --- |
| User | Manages their personal account and adoption listings. Can buy paid adoption visibility and use the available customer flows. |
| Business | Manages an authorized organization profile, membership, and its own resources. Can publish paid adoption listings and enable the approved store, service, or transport capabilities as those phases open. |
| Superadmin | Administers all application modules, including users, organization enrollment, moderation, prices, terms, and authorized financial actions. Can publish and pay for its own adoption listing. Require strong authentication and audit sensitive actions. |

Use one Business role for both Foundation and Store profile types. A Business can operate without a store. Authorize role, capability, and resource scope in the backend for every operation, including lists, searches, downloads, and direct resource access. Scope Business operations to the authorized organization and preserve account and operation history during profile conversion. Audit sensitive actions with actor, action, target, timestamp, and reason.

## Phase 1 — Adoptions

### Registration, locations, and Business enrollment

Ask for name, email, password, country, and city at personal registration. Preselect Colombia and enable it as the initial country. Use the approved location catalog to support several cities from launch and future countries through configuration. Group Colombian locations by department in the selector. Keep official DIVIPOLA codes as text and validate country, department, and location on the server. Store a listing's location independently from the account's city. [DANE catalog and import notes](https://github.com/erickvalderrama/it/tree/main/prompts/mascotas/data)

Start Business enrollment with a choice of store or foundation for both new accounts and conversions of existing accounts. Request store identity evidence when the person enrolls as a store or later enables a storefront. For a natural person request a Colombian identity card and RUT. For a legal entity request NIT and RUT, with representation checks where applicable. Keep these documents private, restrict verification access, and minimize retention. Apply the same Business membership rules to both profile types.

Support free promotional enrollment for eligible foundations and future stores with configurable eligibility, start, and end dates. A free year from Business activation is a pilot proposal. Offer the assigned monthly plan and require explicit acceptance of automatic monthly billing before the first charge. Show status, benefits, expiry, next charge date, and price in the Business profile. Renew through the automatic subscription billing described below. Honor existing obligations after membership expiry.

### Adoption listing, filters, and review

Provide basic and paid adoption listings. User, Business, and Superadmin can buy paid listings. Record whether the publisher acted as an individual or for an organization at publication time. Preserve that source after role or profile changes and filter public results by city and publisher type. Show the publisher, pet profile, adoption contact or application action, and listing status. Allow publishers to manage their own listings and adoption requests. The authorized publisher decides the adoption and handles the animal's handover.

Show the fee, benefits, duration, content rules, and refund conditions for the paid listing. Mascotas receives the charge for the listing or its visibility. The publisher handles the adoption separately. For a paid listing, charge first and submit its exact revision for review after server-side ePayco confirmation. Make the listing visible and start the paid period only after approval. Track review, publication, payment, invoicing, and refund as separate states.

Use an approved multimodal AI provider such as OpenAI API behind a backend port. Review the listing text, each image, and visible text within images. Send only the necessary sanitized content and strip image metadata. Use approved moderation rules and structured, validated assessments. Verify policy coverage for each input type and require a result for every submitted image. Keep identity documents, billing data, and private contact details outside the review request.

- Approve automatically when all required text and images pass the rules.
- Request corrections for fixable issues. Identify the field or photo and explain how to fix it. Let the publisher revise and resubmit the same paid listing using the original payment and accepted price.
- Reject the submitted revision for clear vulgarity, any sexual content in text or images, or another clear policy violation. Keep the revision hidden and tell the publisher why. Allow a clean replacement where the policy permits it.
- Route uncertainty, unreadable or missing image review, inconsistent responses, and persistent provider failure to Superadmin. Record the decision and its reason.

Re-review all content in each submitted revision. Keep unapproved new listings private and prevent stale review results from activating another revision. Record content revision, policy version, assessment by input, provider, outcome, and human decision. Minimize retention of prohibited material and keep it out of notifications. The pilot proposes a full refund for final rejection before activation. Confirm supported refund methods and publish the correction and refund terms before charging. Link each refund and applicable invoice adjustment to the original transaction and apply it once.

### Versioned prices, ePayco, and electronic invoicing

Keep a dedicated history of immutable price versions, with amounts, currency, product or membership scope, effective dates, and assigned Business where applicable. Create a new version when a price changes. Record the exact version, accepted total, currency, applicable benefits, timestamp, and actor for each publication or payment, including zero-price offers. Link the listing, payment, membership period, and invoice to the corresponding accepted offer. Support querying the latest accepted price while retaining every prior acceptance. Validate and freeze an accepted quote server-side for its stated lifetime. Resolve quote expiry and late payment confirmations explicitly.

Use ePayco for listing and Business charges through a backend adapter. Verify each notification using the provider's documented mechanism and reconcile amount, currency, reference, and operation. Use the authenticated server result for the final status. Apply a confirmation once even if it arrives repeatedly or concurrently.

For named invoices, ask for the permitted buyer identification data at checkout and offer the applicable final-consumer flow. Keep phone and address separate when needed for an operational purpose and apply current DIAN requirements to invoice issuance. Issue electronic invoices for Mascotas listing and membership charges through the approved DIAN-enabled flow. Keep the ePayco payment confirmation and electronic invoice linked with separate identifiers and states. Record issuance, validation, delivery, retries, and applicable corrections. Use an authorized refund flow when required.

### Subscriptions with ePayco tokenization and automatic monthly charges

Owner rule, 11 November 2026. Charge monthly subscriptions automatically using customer tokenization in ePayco.
- Each subscription plan has a monthly price and a monthly quantity of publications. Store plans as immutable price versions. Link each subscription, charge, invoice, and consumed publication to the exact plan version accepted.
- Count publications against the active period's quota. Show used and remaining publications. Confirm rollover rules, upgrades, downgrades, and charges beyond the quota with the owner before enabling them.
- Require these subscriber fields before subscribing: full names, identity document type, identity document number, address, phone, and email. Validate them on the server: allowed document types and number formats, normalized phone with country code, verified email, and a complete address. Correct data is required so the monthly charge does not fail.
- Tokenize the card through ePayco's client-side tokenization or hosted flow. Never send, store, or log card numbers or security codes in Petyei. Store only the ePayco customer and token references with restricted access.
- Record explicit consent for automatic recurring charges, with amount, frequency, plan version, and cancellation method. Let the subscriber cancel future charges at any time from the account.
- Run monthly charges as idempotent persisted jobs in the backend. Confirm each result through the verified server notification or status query. Define bounded retries, payment failure notices, a grace period, suspension, and reactivation after the card or data is updated.
- Issue the electronic invoice for each successful charge and reconcile it with the ePayco transaction.
- Confirm in the current ePayco documentation and with the account manager the tokenization, customer, plan, and recurring charge APIs available to the merchant account before implementation.

The pilot proposal starts paid listings and memberships on responsive web. Review Apple and Google Play rules for digital purchases before enabling them in store-distributed mobile apps. Treat physical service and product checkout as a separate review.

### Phase 1 screens and administration

Deliver registration, city and publisher filters, a shareable listing detail, listing draft and preview, review and payment status, Business enrollment and membership, terms acceptance, invoice access, and a Superadmin dashboard. Reuse the same states and controls on compact and wide layouts. Show registered, enabled, and active user counts, filterable by city, role, and period. Display the definition and period of the active user count. Include review, Business enrollment, pricing, terms versions, and payment or invoice recovery in the dashboard. Keep administrative capabilities audited.

## Phase 2 — Stores and pet services

Give a verified Business a store dashboard with subscription status, catalog, stock, orders, and its own delivery settings. Publish merchant name, availability, product details, price, delivery cost, and expected delivery before checkout. The store fulfills the order and handles delivery and product support. Mascotas handles the platform charges and functions it undertakes. Preserve access needed to fulfill orders already accepted.

Support paid pet bathing, washing, cleaning, grooming, and beauty at home or through a scheduled booking. Display service, location, provider, date, time slot, inclusions, total, and cancellation terms. Reconcile slot holds with confirmed payment. Handle expiry, late confirmations, failure, cancellation, and refunds with consistent booking and payment states.

Integrate ePayco Payouts for eligible verified beneficiaries and reconcile each payout with the underlying operation. Show the complete total and the party responsible for fulfillment before confirmation. Use exact monetary representation, explicit rounding, and idempotent payment, refund, inventory, balance, and payout operations.

Support customer top-ups for eligible small purchases and separate Business top-ups for commissions. Keep available, reserved, and consumed balance and provider payables traceable. Apply each agreed fee once. Obtain approval for the custody, terms, billing, refund, and provider rules before enabling balances.

## Phase 3 — Pet transport from A to B

Add the transport capability to Business after applicable operator, vehicle, and coverage verification. Before paid trips, resolve the Colombian transport modality and authorization for the actual service, contract, animal welfare and custody, insurance coverage, and incident protocol.

Capture pickup A, destination B, pet requirements, time, pickup and delivery contacts, and the customer's proposed budget. Providers can accept the proposal or submit counteroffers. Show inclusions, one-way or return arrangements, waiting conditions, price, and the provider selected by the customer. Show request, accepted offer, payment, pickup, active trip, delivery, and cancellation consistently. Confirm pickup and delivery with the responsible participants.

Share exact addresses and active location with authorized participants. Start tracking from an explicit action during an active trip and stop on completion or cancellation. Show the last received position and time. Test locked screens, permission changes, loss of connectivity, retries, and recovery on supported mobile devices.

Reconcile the trip charge, platform commission, provider amount, payout, and refund where applicable. Support approved small-payment or commission top-ups with separate, auditable balances.

## Security priority

Treat authorization, data protection, and payment integrity as acceptance criteria.
- Authenticate on the server and enforce least privilege on every resource. Validate object ownership and role changes, including direct requests made outside Flutter.
- Protect account recovery, administrative access, authentication endpoints, and payment notifications. Require stronger authentication for privileged users and revoke access when roles or sessions change.
- Store credentials in secure environment configuration. Use encrypted transport and restrict browser origins to the approved frontend origins.
- Validate input, uploads, filenames, and outgoing integrations on the server. Verify image format and size, strip metadata from public and moderation copies, and remove abandoned uploads under the retention policy. Store only necessary personal data. Protect identity documents, private addresses, location, and payment references with restricted access.
- Keep secrets and sensitive data out of code, responses, and logs. Record audit events and safe correlation references for investigations and refunds.

### Authentication tokens and passwords

Owner rules, 11 November 2026.
- **JWT access tokens do not expire.** Issue signed JWTs without an expiration claim. Because a non-expiring token remains valid if it is stolen, every token must carry a session identifier, and the backend must validate that session on each request. Revoke the session on logout, password change, role or capability change, account suspension, logical deletion, and suspected compromise. Let users and Superadmin close sessions per device. Rotate signing keys through configuration with a documented procedure. Store tokens on mobile in secure platform storage and protect web storage against cross-site scripting.
- **Passwords are stored only as one-way hashes.** Use ASP.NET Core Identity's password hasher or Argon2id with a per-password salt and current work factors. Never store passwords in plain text or with reversible encryption, and never log them. Rehash on sign-in when parameters change. Enforce the minimum password policy and rate-limit sign-in and recovery.

## Own machine learning model — mandatory

Owner rule, 11 November 2026. Petyei must include its own machine learning model, trained and served by the project. A third-party AI API does not satisfy this requirement; the moderation provider remains a separate component.
- **Initial use case: adopter and pet compatibility.** The owner's master's thesis proposed matching adopters and pets from questionnaires with ML.NET. Start from that design: adopter questionnaire, pet questionnaire, species and breed catalog, and a compatibility score shown on listings and adoption requests. Source: `documents/tesis/` (final draft, ERD, species and breeds spreadsheet, user stories, and the "Mascotas en tu casa" survey). Use the anonymized [survey summary and question bank](documents/prompts/mascotas/model/survey-summary.md); never copy the raw survey, which contains respondents' email addresses, into the application, fixtures, or training data.
- Use ML.NET inside the backend. Train from real platform signals such as questionnaire answers, adoption requests, publisher decisions, and completed adoptions. The thesis proof of concept used randomly generated scores, so its reported metrics do not validate the model.
- Until enough labeled data exists, rank with a transparent rule-based baseline and collect the training signals. Train the model when the agreed data threshold is reached and promote it only when it outperforms the baseline on held-out data.
- Version datasets, features, training code, model artifacts in MinIO, metrics, and the active model in PostgreSQL. Train as a backend job, load the active model in the API, and allow rollback to the previous model.
- Explain the main reasons for a score in plain language. The score orders and suggests; the publisher still decides the adoption. Exclude identity documents, payment data, and exact addresses from features. Collect consent for questionnaire data and describe the model in the privacy policy.
- Test feature extraction, training reproducibility with fixed seeds, evaluation against the baseline, prediction latency, and fallback when no model is active.

## Terms, privacy, and user consent

### Versions by phase and language

Owner rule, 11 November 2026. Write the Terms and Conditions and the Privacy Policy for the scope that is actually enabled.
- Version 1 covers Phase 1 only: accounts, adoption listings, paid listings, Business enrollment and membership, subscriptions with automatic monthly charges, AI moderation, the compatibility model and its questionnaires, invoicing, and support. Do not describe stores, services, top-ups, payouts, or transport as available in version 1.
- Publish a new version before enabling Phase 2 and another before Phase 3, adding only the functions that phase enables. Require acceptance of each new version before the new operations, under the rules below.
- Publish every version in Spanish and English with the same version number, effective date, and content. Let users read and download either language from onboarding, transactions, settings, and a public page. Record the language shown and accepted. State in both texts which language prevails in case of discrepancy, as confirmed in legal review.
- A future language adds a translation of the current version; it does not create a new version.

Keep immutable versions of the Terms and Conditions and Privacy Policy with language, text, version, and effective date. Make them accessible for reading and download from onboarding, relevant transactions, and account settings. Record the exact document version, user, and acceptance time in the backend. Preserve the full history and the latest accepted version for each document. Collect personal data authorization separately when required.

When updated Terms apply, present them at sign-in or before a protected flow. Require explicit acceptance and enforce it server-side before new publications and payments. Preserve access to legal documents, existing operations, and support. Associate each accepted operation with the legal version applicable to it.

In the legal copy, specify that Mascotas provides the advertised digital tools and is responsible for the functions and charges it undertakes. The publisher manages and is responsible for the adoption information and handover, the store handles its sale and delivery, and the independent provider performs the agreed service. Identify the responsible party in each flow and preserve mandatory consumer and data rights. Describe AI-assisted moderation, membership pricing, renewals, complaints, cancellations, and refunds as they apply to the enabled features.

Use the reviewed versions in the future project under `docs/legal/v1/es/` and `docs/legal/v1/en/` (`terms-and-conditions.md` and `privacy-policy.md`), with later phases in `v2`, `v3`, and so on. The current drafts are in Spanish and cover all three phases; adapt them to Phase 1 before publishing. Complete operator identity, support channels, effective dates, retention, and actual payment and data practices before publishing. Use the initial Colombia drafts for legal review.
- [Terms and conditions draft](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/legal/terms-and-conditions.md)
- [Privacy policy draft](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/legal/privacy-policy.md)

## Required tests and delivery checks

Write unit tests in C# and Dart for new and changed behavior. Cover role and ownership rules, use cases, Riverpod state transitions, monetary calculations, membership eligibility, location validation, per-image and text moderation, sexual and vulgar content rejection, corrections with the original payment, price and Terms version changes, listing activation, refunds, invoice retry, booking, cancellation, and duplicate notifications when touched.
Run API and persistence integration tests in an isolated test database on the authorized external PostgreSQL service for authorization, contracts, migrations, constraints, transactions, and concurrent writes. Use an isolated MinIO test bucket or prefix for storage integration and reset fixtures only on validated test targets. Use Flutter widget tests for responsive components, validation, accessibility semantics, and recoverable UI states. Add a regression test for each fixed defect at the most direct layer that reproduces it.

### End-to-end flow tests

Run critical flows through the built Flutter app, API, PostgreSQL, and applicable background workers. Use Flutter `integration_test` or a compatible runner. Run web tests in a browser and mobile tests on emulators, simulators, or devices. Cover browser refresh, deep links, and checkout return on enabled targets. Verify system permissions and background behavior on the relevant mobile platforms. [Flutter integration testing](https://docs.flutter.dev/testing/integration-tests)

Use synthetic fixtures, isolated test accounts and data, a controlled clock, and stable widget keys or accessible selectors. Wait for observable states with bounded timeouts. Simulate provider responses at the integration boundary in CI, including ePayco notifications, invoice issuance, and AI results per text and image. Run separate staging smoke checks against the authorized test environments available for each provider.

- Phase 1 access and discovery. Register with Colombia preselected, select and change a valid DIVIPOLA location, and enroll each Business profile type. Verify that updated Terms are required before a new protected operation and that the previous acceptance remains available. Publish a basic adoption, filter by city and publisher type, and verify that one account cannot view or change another account's private records.
- Phase 1 premium. Accept the versioned price, complete checkout, confirm payment on the server, review text and every photo, and verify visibility and the invoice. Cover payment pending or failed, duplicate or late confirmation, corrections and resubmission with the same payment, rejection for sexual or vulgar content, uncertain results sent to Superadmin, and invoice retry. Assert a single applied payment, the original accepted price, and no public listing before approval.
- Phase 1 membership and administration. Cover Business promotion, expiry and renewal at the offered price, access to historical operations, and authorized review and payment or invoice recovery.
- Phase 1 subscriptions. Subscribe with complete and invalid billing data, tokenize through the ePayco test flow, run the monthly charge with a controlled clock, consume and exhaust the publication quota, and cover a failed charge, retry, grace period, suspension, card update, and cancellation. Assert one charge and one invoice per period.
- Languages, themes, and legal versions. Switch between English and Spanish and between light and dark on web and mobile without losing the current route or form data. Verify fallback to English, localized server error messages and emails, and catalog translations. Read and accept version 1 of the legal documents in each language and confirm the recorded version and language.
- Platform rules. Verify that delete operations change state and hide records without removing rows, that revoked sessions reject non-expiring tokens, that password hashes are never returned or logged, and that migrations create the expected indexes.
- Compatibility model. Answer both questionnaires, get the baseline score, train a model on synthetic labeled fixtures, promote it, and fall back to the baseline when the model is missing.
- Phase 2 commerce. Cover a store order with delivery and stock changes, a booking with slot conflicts and cancellation, approved top-up and balance use, and payout or refund retry with reconciliation.
- Phase 3 transport. Cover a request and counteroffer, acceptance, payment, pickup, delivery, and reconciliation. Verify authorized tracking, denied permissions, background and locked-screen behavior, connection loss, and recovery.

### Visual regression and accessibility

- Capture goldens for critical screens in English and Spanish and in light and dark themes.
- Create reviewed golden images or equivalent screenshot baselines for critical screens and reusable components. Start with registration, adoption results and detail, checkout and review states, and the Superadmin queue. Capture compact, medium, and wide layouts and applicable loading, empty, error, and success states. [Flutter golden tests](https://api.flutter.dev/flutter/flutter_test/matchesGoldenFile.html)
- Pin the rendering environment, fonts, viewport, pixel density, locale, theme, and test data. Store baselines with the tests and compare expected, actual, and diff images in CI. Review the visual difference and document intentional design changes before updating a baseline.
- Check overflow, clipping, long labels, enlarged text, scrolling, keyboard and safe areas, focus order, contrast, and screen reader labels. Review the actual app at representative web and mobile sizes, including cases that require device or manual testing.

## DevOps and release readiness

- Separate local, CI, staging, and production configuration, storage, credentials, and callbacks. Provision a repeatable staging environment and use synthetic data and test credentials for automated runs.
- Check migrations on an empty database and on representative existing data. Plan app and schema compatibility, backups, a tested restore, and a rollback or forward repair for a failed rollout.
- Monitor health and readiness, application errors, and stalled payment, moderation, and invoice processing. Use redacted logs and safe correlation references. Document how to diagnose and recover a stuck operation and verify its outcome.
- In CI, run `dotnet build`, `dotnet test`, Dart format checks, `flutter analyze`, `flutter test`, relevant builds and golden tests, and critical web E2E for changes that affect those flows. Run the relevant Android and iOS E2E and staging smoke tests before releasing those targets. Retain sanitized logs and visual diffs for failures.
- Require passing critical E2E and visual checks for the enabled scope before each phase release. Verify the 699-line limit, English code, centralized UI copy, and approved model mappings. Record commands, tested targets, actual results, and any remaining coverage gaps.
