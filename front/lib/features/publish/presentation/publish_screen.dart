import 'package:flutter/material.dart';

import '../../../core/l10n/l10n.dart';
import '../../../core/widgets/status_message.dart';

/// Entry point for adoption drafts. The draft form arrives with delivery 1B.
class PublishScreen extends StatelessWidget {
  const PublishScreen({super.key});

  @override
  Widget build(BuildContext context) {
    final l10n = context.l10n;
    return Scaffold(
      appBar: AppBar(title: Text(l10n.publishTitle)),
      body: StatusMessage(message: l10n.publishIntro),
    );
  }
}
