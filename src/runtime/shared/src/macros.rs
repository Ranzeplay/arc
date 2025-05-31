#[macro_export]
macro_rules! receive_func_args {
    ($stack:ident, $($type:ty),*) => {
        (
            $(
                TryInto::<$type>::try_into($stack.pop().expect("Stack is empty").borrow().clone())
                    .expect("Failed to convert into target type")
            ),*
        )
    };
}
