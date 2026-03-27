import React from "react";

export default function NotFound() {
	return (
		<div className="p-8 max-w-2xl mx-auto text-center">
			<h1 className="text-2xl font-semibold">Page Not Found</h1>
			<p className="mt-3 text-muted-fg">We couldn't find the page you're looking for.</p>
			<a href="/" className="inline-block mt-6 px-4 py-2 bg-muted text-muted-fg rounded-md border border-border">Return to Dashboard</a>
		</div>
	);
}
