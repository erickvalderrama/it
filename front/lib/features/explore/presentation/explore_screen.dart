import 'package:flutter/material.dart';

import '../../../core/l10n/l10n.dart';
import '../../../core/widgets/status_message.dart';

/// Adoption search. Listings, filters, and cards arrive with delivery 1B.
class ExploreScreen extends StatelessWidget {
  const ExploreScreen({super.key});

  @override
  Widget build(BuildContext context) {
    final l10n = context.l10n;
    return Scaffold(
      appBar: AppBar(title: Text(l10n.exploreTitle)),
      body: StatusMessage(message: l10n.exploreEmpty),
    );
  }
}
