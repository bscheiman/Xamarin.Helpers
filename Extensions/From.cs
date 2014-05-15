//--------------------------------------------------------------------------
//
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//
//  File: TaskFactoryExtensions_From.cs
//
//--------------------------------------------------------------------------

namespace System.Threading.Tasks {
	/// <summary>Extensions for TaskFactory.</summary>
	public static partial class TaskFactoryExtensions {
		/// <summary>Transfers the result of a Task to the TaskCompletionSource.</summary>
		/// <typeparam name="TResult">Specifies the type of the result.</typeparam>
		/// <param name="resultSetter">The TaskCompletionSource.</param>
		/// <param name="task">The task whose completion results should be transfered.</param>
		public static void SetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task task) {
			switch (task.Status) {
				case TaskStatus.RanToCompletion:
					resultSetter.SetResult(task is Task<TResult> ? ((Task<TResult>)task).Result : default(TResult));
					break;
				case TaskStatus.Faulted:
					resultSetter.SetException(task.Exception.InnerExceptions);
					break;
				case TaskStatus.Canceled:
					resultSetter.SetCanceled();
					break;
				default:
					throw new InvalidOperationException("The task was not completed.");
			}
		}

		/// <summary>Transfers the result of a Task to the TaskCompletionSource.</summary>
		/// <typeparam name="TResult">Specifies the type of the result.</typeparam>
		/// <param name="resultSetter">The TaskCompletionSource.</param>
		/// <param name="task">The task whose completion results should be transfered.</param>
		public static void SetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task<TResult> task) {
			SetFromTask(resultSetter, (Task)task);
		}

		/// <summary>Attempts to transfer the result of a Task to the TaskCompletionSource.</summary>
		/// <typeparam name="TResult">Specifies the type of the result.</typeparam>
		/// <param name="resultSetter">The TaskCompletionSource.</param>
		/// <param name="task">The task whose completion results should be transfered.</param>
		/// <returns>Whether the transfer could be completed.</returns>
		public static bool TrySetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task task) {
			switch (task.Status) {
				case TaskStatus.RanToCompletion:
					return resultSetter.TrySetResult(task is Task<TResult> ? ((Task<TResult>)task).Result : default(TResult));
				case TaskStatus.Faulted:
					return resultSetter.TrySetException(task.Exception.InnerExceptions);
				case TaskStatus.Canceled:
					return resultSetter.TrySetCanceled();
				default:
					throw new InvalidOperationException("The task was not completed.");
			}
		}

		/// <summary>Attempts to transfer the result of a Task to the TaskCompletionSource.</summary>
		/// <typeparam name="TResult">Specifies the type of the result.</typeparam>
		/// <param name="resultSetter">The TaskCompletionSource.</param>
		/// <param name="task">The task whose completion results should be transfered.</param>
		/// <returns>Whether the transfer could be completed.</returns>
		public static bool TrySetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task<TResult> task) {
			return TrySetFromTask(resultSetter, (Task)task);
		}

#region TaskFactory
		/// <summary>Creates a generic TaskFactory from a non-generic one.</summary>
		/// <typeparam name="TResult">Specifies the type of Task results for the Tasks created by the new TaskFactory.</typeparam>
		/// <param name="factory">The TaskFactory to serve as a template.</param>
		/// <returns>The created TaskFactory.</returns>
		public static TaskFactory<TResult> ToGeneric<TResult>(this TaskFactory factory) {
			return new TaskFactory<TResult>(
				factory.CancellationToken, factory.CreationOptions, factory.ContinuationOptions, factory.Scheduler);
		}

		/// <summary>Creates a generic TaskFactory from a non-generic one.</summary>
		/// <typeparam name="TResult">Specifies the type of Task results for the Tasks created by the new TaskFactory.</typeparam>
		/// <param name="factory">The TaskFactory to serve as a template.</param>
		/// <returns>The created TaskFactory.</returns>
		public static TaskFactory ToNonGeneric<TResult>(this TaskFactory<TResult> factory) {
			return new TaskFactory(
				factory.CancellationToken, factory.CreationOptions, factory.ContinuationOptions, factory.Scheduler);
		}

		/// <summary>Gets the TaskScheduler instance that should be used to schedule tasks.</summary>
		public static TaskScheduler GetTargetScheduler(this TaskFactory factory) {
			if (factory == null)
				throw new ArgumentNullException("factory");
			return factory.Scheduler ?? TaskScheduler.Current;
		}

		/// <summary>Gets the TaskScheduler instance that should be used to schedule tasks.</summary>
		public static TaskScheduler GetTargetScheduler<TResult>(this TaskFactory<TResult> factory) {
			if (factory == null)
				throw new ArgumentNullException("factory");
			return factory.Scheduler != null ? factory.Scheduler : TaskScheduler.Current;
		}

		/// <summary>Converts TaskCreationOptions into TaskContinuationOptions.</summary>
		/// <param name="creationOptions"></param>
		/// <returns></returns>
		private static TaskContinuationOptions ContinuationOptionsFromCreationOptions(TaskCreationOptions creationOptions) {
			return (TaskContinuationOptions)
				((creationOptions & TaskCreationOptions.AttachedToParent) |
			(creationOptions & TaskCreationOptions.PreferFairness) |
			(creationOptions & TaskCreationOptions.LongRunning));
		}

		/// <summary>Creates a Task that has completed in the Faulted state with the specified exception.</summary>
		/// <param name="factory">The target TaskFactory.</param>
		/// <param name="exception">The exception with which the Task should fault.</param>
		/// <returns>The completed Task.</returns>
		public static Task FromException(this TaskFactory factory, Exception exception) {
			var tcs = new TaskCompletionSource<object>(factory.CreationOptions);
			tcs.SetException(exception);
			return tcs.Task;
		}

		/// <summary>Creates a Task that has completed in the Faulted state with the specified exception.</summary>
		/// <typeparam name="TResult">Specifies the type of payload for the new Task.</typeparam>
		/// <param name="factory">The target TaskFactory.</param>
		/// <param name="exception">The exception with which the Task should fault.</param>
		/// <returns>The completed Task.</returns>
		public static Task<TResult> FromException<TResult>(this TaskFactory factory, Exception exception) {
			var tcs = new TaskCompletionSource<TResult>(factory.CreationOptions);
			tcs.SetException(exception);
			return tcs.Task;
		}

		/// <summary>Creates a Task that has completed in the RanToCompletion state with the specified result.</summary>
		/// <typeparam name="TResult">Specifies the type of payload for the new Task.</typeparam>
		/// <param name="factory">The target TaskFactory.</param>
		/// <param name="result">The result with which the Task should complete.</param>
		/// <returns>The completed Task.</returns>
		public static Task<TResult> FromResult<TResult>(this TaskFactory factory, TResult result) {
			var tcs = new TaskCompletionSource<TResult>(factory.CreationOptions);
			tcs.SetResult(result);
			return tcs.Task;
		}

		/// <summary>Creates a Task that has completed in the Canceled state with the specified CancellationToken.</summary>
		/// <param name="factory">The target TaskFactory.</param>
		/// <param name="cancellationToken">The CancellationToken with which the Task should complete.</param>
		/// <returns>The completed Task.</returns>
		public static Task FromCancellation(this TaskFactory factory, CancellationToken cancellationToken) {
			if (!cancellationToken.IsCancellationRequested)
				throw new ArgumentOutOfRangeException("cancellationToken");
			return new Task(() => {
			}, cancellationToken);
		}

		/// <summary>Creates a Task that has completed in the Canceled state with the specified CancellationToken.</summary>
		/// <typeparam name="TResult">Specifies the type of payload for the new Task.</typeparam>
		/// <param name="factory">The target TaskFactory.</param>
		/// <param name="cancellationToken">The CancellationToken with which the Task should complete.</param>
		/// <returns>The completed Task.</returns>
		public static Task<TResult> FromCancellation<TResult>(this TaskFactory factory, CancellationToken cancellationToken) {
			if (!cancellationToken.IsCancellationRequested)
				throw new ArgumentOutOfRangeException("cancellationToken");
			return new Task<TResult>(DelegateCache<TResult>.DefaultResult, cancellationToken);
		}

		/// <summary>A cache of delegates.</summary>
		/// <typeparam name="TResult">The result type.</typeparam>
		private class DelegateCache<TResult> {
			/// <summary>Function that returns default(TResult).</summary>
			internal static readonly Func<TResult> DefaultResult = () => default(TResult);
		}
#endregion

#region TaskFactory<TResult>
		/// <summary>Creates a Task that has completed in the Faulted state with the specified exception.</summary>
		/// <param name="factory">The target TaskFactory.</param>
		/// <param name="exception">The exception with which the Task should fault.</param>
		/// <returns>The completed Task.</returns>
		public static Task<TResult> FromException<TResult>(this TaskFactory<TResult> factory, Exception exception) {
			var tcs = new TaskCompletionSource<TResult>(factory.CreationOptions);
			tcs.SetException(exception);
			return tcs.Task;
		}

		/// <summary>Creates a Task that has completed in the RanToCompletion state with the specified result.</summary>
		/// <typeparam name="TResult">Specifies the type of payload for the new Task.</typeparam>
		/// <param name="factory">The target TaskFactory.</param>
		/// <param name="result">The result with which the Task should complete.</param>
		/// <returns>The completed Task.</returns>
		public static Task<TResult> FromResult<TResult>(this TaskFactory<TResult> factory, TResult result) {
			var tcs = new TaskCompletionSource<TResult>(factory.CreationOptions);
			tcs.SetResult(result);
			return tcs.Task;
		}

		/// <summary>Creates a Task that has completed in the Canceled state with the specified CancellationToken.</summary>
		/// <typeparam name="TResult">Specifies the type of payload for the new Task.</typeparam>
		/// <param name="factory">The target TaskFactory.</param>
		/// <param name="cancellationToken">The CancellationToken with which the Task should complete.</param>
		/// <returns>The completed Task.</returns>
		public static Task<TResult> FromCancellation<TResult>(this TaskFactory<TResult> factory, CancellationToken cancellationToken) {
			if (!cancellationToken.IsCancellationRequested)
				throw new ArgumentOutOfRangeException("cancellationToken");
			return new Task<TResult>(DelegateCache<TResult>.DefaultResult, cancellationToken);
		}
#endregion
	}
}